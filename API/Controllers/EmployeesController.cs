using Application.DTOs.Employee;
using Application.DTOs.Publisher;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.AppServices;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.EmployeeSpec;
using Infrastructure.Specifications.PayrollSpec;
using Infrastructure.Specifications.PublisherSpec;
using Infrastructure.Specifications.VacationSpec;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Application.DTOs.Author;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly IEmployeeServices _employeeServices;
        private readonly ILogger<EmployeesController> _logger;
        
        public EmployeesController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IPhoneNumberValidator phoneNumberValidator,
                                    IEmployeeServices employeeServices,
                                    ILogger<EmployeesController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _employeeServices = employeeServices;
            _logger = logger;
        }


        #region GET
        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadEmployeeDto>>> GetAllEmployeesAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));

            var spec = new EmployeeSpec(pagesize, pageindex, isPagingEnabled);

            var totalEmployees = await _uof.GetRepository<Employee>().CountAsync(spec);
            
            var employees = await _uof.GetRepository<Employee>().FindAllSpec(spec);

            var mappedEmployees = _mapper.Map<IReadOnlyList<ReadEmployeeDto>>(employees);

            if (mappedEmployees == null && totalEmployees == 0)
            {
                return NotFound(new ApiResponse(404, AppMessages.NULL_DATA));
            }
            var paginationData = new Pagination<ReadEmployeeDto>(spec.Skip, spec.Take, totalEmployees, mappedEmployees);

            return Ok(paginationData);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeByIdAsync(string id)
        {
            if (await _uof.GetRepository<Employee>().Exists(int.Parse(id)))
            {
                var employee = await _uof.GetRepository<Employee>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Employee, ReadEmployeeDto>(employee));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> SearchEmployeeWithCriteria(string name = null, string type = null, string phone = null, string salary = null)
        {
            if (!_phoneNumberValidator.IsValidPhoneNumber(phone))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            if (!_employeeServices.IsValidEmployeeType(byte.Parse(type)))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_EMPTYPE));
            var result = await _employeeServices.SearchEmployeeDataWithDetail(name, byte.Parse(type), phone, decimal.Parse(salary));
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404, AppMessages.NOTFOUND_SEARCHDATA));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [Authorize(Roles = "Manager,HR")]
        [HttpPost]
        public async Task<ActionResult> InsertEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            if (!_employeeServices.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_EMPTYPE));
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_AGE));
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().InsertAsync(employee);
            await _uof.Commit();

            return Ok(new ApiResponse(201, AppMessages.INSERTED));
        }
        #endregion

        #region PUT
        [Authorize(Roles = "Manager,HR")]
        [HttpPut]
        public async Task<ActionResult> UpdateEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(employeeDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_employeeServices.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_EMPTYPE));
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_AGE));
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().UpdateAsync(employee);
            await _uof.Commit();

            return Ok(new ApiResponse(201, AppMessages.UPDATED));
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "Manager,HR")]
        [HttpDelete]
        public async Task<ActionResult> DeleteEmployeeAsync(string id)
        {
            var attendanceSpec = new AttendanceWithEmployeeSpec(null, int.Parse(id));
            var UsedInAttendance = _uof.GetRepository<Attendence>().FindAllSpec(attendanceSpec).Result;
            
            var payrollSpec = new PayrollWithEmployeeSpec(null, int.Parse(id));
            var UsedInPayroll = _uof.GetRepository<Payroll>().FindAllSpec(payrollSpec).Result;

            var vacationSpec = new VacationWithEmployeeSpec(null, int.Parse(id));
            var UsedInVacation = _uof.GetRepository<Vacation>().FindAllSpec(vacationSpec).Result;

            if (UsedInAttendance.Count() > 0 || UsedInPayroll.Count() > 0 || UsedInVacation.Count() > 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.FAILED_DELETE));
            }
            else
            {
                var employeeSpec = new EmployeeSpec(int.Parse(id));
                var employee = _uof.GetRepository<Employee>().FindSpec(employeeSpec).Result;
                _uof.GetRepository<Employee>().DeleteAsync(employee);
                await _uof.Commit();
                return Ok(new ApiResponse(201, AppMessages.DELETED));
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<ActionResult> FireEmployeeAsync(string id)
        {
            var result = await _uof.GetRepository<Employee>().Exists(int.Parse(id));
            if (!result)
            {
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            }
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(int.Parse(id));
            _uof.GetRepository<Employee>().DeleteAsync(employee);

            var attendanceSpec = new AttendanceWithEmployeeSpec(null, int.Parse(id));
            var AttendanceRecords = _uof.GetRepository<Attendence>().FindAllSpec(attendanceSpec).Result;
            if (AttendanceRecords != null)
                _uof.GetRepository<Attendence>().DeleteRangeAsync(AttendanceRecords);

            var payrollSpec = new PayrollWithEmployeeSpec(null, int.Parse(id));
            var PayrollRecords = _uof.GetRepository<Payroll>().FindAllSpec(payrollSpec).Result;
            if (PayrollRecords != null)
                _uof.GetRepository<Payroll>().DeleteRangeAsync(PayrollRecords);

            var vacationSpec = new VacationWithEmployeeSpec(null, int.Parse(id));
            var VacationRecords = _uof.GetRepository<Vacation>().FindAllSpec(vacationSpec).Result;
            if (VacationRecords != null)
                _uof.GetRepository<Vacation>().DeleteRangeAsync(VacationRecords);

            await _uof.Commit();
            return Ok(new ApiResponse(201, AppMessages.FIRED));
        }
        #endregion
    }
}
