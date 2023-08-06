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

namespace API.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> GetAllEmployeesAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400));

            var spec = new EmployeeSpec(pagesize, pageindex, isPagingEnabled);

            var totalEmployees = await _uof.GetRepository<Employee>().CountAsync(spec);
            if(totalEmployees == 0)
                return NotFound(new ApiResponse(404));

            var employees = await _uof.GetRepository<Employee>().FindAllSpec(spec);

            var mappedEmployees = _mapper.Map<IReadOnlyList<ReadEmployeeDto>>(employees);

            var paginationData = new Pagination<ReadEmployeeDto>(spec.PageIndex, spec.PageSize, totalEmployees, mappedEmployees);

            return Ok(paginationData);
        }


        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult> GetEmployeeByIdAsync(int id)
        {
            if (await _uof.GetRepository<Employee>().Exists(id))
            {
                var employee = await _uof.GetRepository<Employee>().GetByIdAsync(id);
                return Ok(_mapper.Map<Employee, ReadEmployeeDto>(employee));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }


        [HttpGet("SearchEmployeeWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> SearchWithCriteria(string name = null, byte? type = null, string phone = null, decimal? salary = null)
        {
            var result = await _employeeServices.SearchEmployeeDataWithDetail(name, type, phone, salary);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertEmployee")]
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

            return StatusCode(201, AppMessages.INSERTED); 
        }
        #endregion

        #region PUT
        [HttpPut("UpdateEmployee")]
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

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployeeAsync(int id)
        {
            var attendanceSpec = new AttendanceWithEmployeeSpec(null, id);
            var UsedInAttendance = _uof.GetRepository<Attendence>().FindAllSpec(attendanceSpec).Result;
            
            var payrollSpec = new PayrollWithEmployeeSpec(null, id);
            var UsedInPayroll = _uof.GetRepository<Payroll>().FindAllSpec(payrollSpec).Result;

            var vacationSpec = new VacationWithEmployeeSpec(null, id);
            var UsedInVacation = _uof.GetRepository<Vacation>().FindAllSpec(vacationSpec).Result;

            if (UsedInAttendance.Count() > 0 || UsedInPayroll.Count() > 0 || UsedInVacation.Count() > 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.FAILED_DELETE));
            }
            else
            {
                var employeeSpec = new EmployeeSpec(id);
                var employee = _uof.GetRepository<Employee>().FindSpec(employeeSpec).Result;
                if (employee == null)
                    return NotFound(new ApiResponse(404));
                _uof.GetRepository<Employee>().DeleteAsync(employee);
                await _uof.Commit();
                return Ok(AppMessages.DELETED);
            }
        }

        [HttpDelete("FireEmployee")]
        public async Task<ActionResult> FireEmployeeAsync(int id)
        {
            var result = await _uof.GetRepository<Employee>().Exists(id);
            if (!result)
            {
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            }
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(id);
            _uof.GetRepository<Employee>().DeleteAsync(employee);

            var attendanceSpec = new AttendanceWithEmployeeSpec(null, id);
            var AttendanceRecords = _uof.GetRepository<Attendence>().FindAllSpec(attendanceSpec).Result;
            if (AttendanceRecords != null)
                _uof.GetRepository<Attendence>().DeleteRangeAsync(AttendanceRecords);

            var payrollSpec = new PayrollWithEmployeeSpec(null, id);
            var PayrollRecords = _uof.GetRepository<Payroll>().FindAllSpec(payrollSpec).Result;
            if (PayrollRecords != null)
                _uof.GetRepository<Payroll>().DeleteRangeAsync(PayrollRecords);

            var vacationSpec = new VacationWithEmployeeSpec(null, id);
            var VacationRecords = _uof.GetRepository<Vacation>().FindAllSpec(vacationSpec).Result;
            if (VacationRecords != null)
                _uof.GetRepository<Vacation>().DeleteRangeAsync(VacationRecords);

            await _uof.Commit();
            return Ok(AppMessages.FIRED);
        }
        #endregion
    }
}
