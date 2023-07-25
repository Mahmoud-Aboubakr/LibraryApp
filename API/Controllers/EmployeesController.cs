using Application.DTOs.Employee;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.AppServices;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {/*
        private readonly IUnitOfWork<Employee> _uof;
        private readonly IUnitOfWork<Attendence> _attendanceUof;
        private readonly IUnitOfWork<Payroll> _payrollUof;
        private readonly IUnitOfWork<Vacation> _vacationUof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly IEmployeeServices _employeeServices;
        private readonly ILogger<EmployeesController> _logger;
        
        public EmployeesController(IUnitOfWork<Employee> uof,
                                    IUnitOfWork<Attendence> AttendanceUof,
                                    IUnitOfWork<Payroll> PayrollUof,
                                    IUnitOfWork<Vacation> VacationUof,
                                    IMapper mapper,
                                    IPhoneNumberValidator phoneNumberValidator,
                                    IEmployeeServices employeeServices,
                                    ILogger<EmployeesController> logger)
        {
            _uof = uof;
            _attendanceUof = AttendanceUof;
            _payrollUof = PayrollUof;
            _vacationUof = VacationUof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _employeeServices = employeeServices;
            _logger = logger;
        }


        #region GET
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> GetAllEmployeesAsync()
        {
            var employees = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<ReadEmployeeDto>>(employees));
        }


        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult> GetEmployeeByIdAsync(int id)
        {
            if (await _uof.GetRepository().Exists(id))
            {
                var employee = await _uof.GetRepository().GetByIdAsync(id);
                return Ok(_mapper.Map<Employee, ReadEmployeeDto>(employee));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("SearchEmployeeWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> SearchWithCriteria(string name = null, byte? type = null, string phone = null, decimal? salary = null)
        {
            var result = await _employeeServices.SearchEmployeeDataWithDetail(name, type, phone, salary);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertEmployee")]
        public async Task<ActionResult> InsertEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            if (!_employeeServices.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_EMPTYPE} {employeeDto.EmpType}" });
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_AGE} {employeeDto.EmpAge}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository().InsertAsync(employee);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED); 
        }
        #endregion

        #region PUT
        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var result = await _uof.GetRepository().Exists(employeeDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {employeeDto.Id}" });
            if (!_employeeServices.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_EMPTYPE} {employeeDto.EmpType}" });
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_AGE} {employeeDto.EmpAge}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository().UpdateAsync(employee);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var UsedInAttendance = await _attendanceUof.GetRepository().FindUsingWhereAsync(x => x.EmpId == employeeDto.Id);
            var UsedInPayroll = await _payrollUof.GetRepository().FindUsingWhereAsync(p => p.EmpId == employeeDto.Id);
            var UsedInVacation = await _vacationUof.GetRepository().FindUsingWhereAsync(v => v.EmpId == employeeDto.Id);

            var result = await _uof.GetRepository().Exists(employeeDto.Id);
            if (!result)
            {
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {employeeDto.Id}" });
            }
            else
            {
                if (UsedInAttendance || UsedInPayroll || UsedInVacation)
                {
                    return BadRequest(new { Detail = $"{AppMessages.FAILED_DELETE} {employeeDto.Id}" });
                }
                else
                {
                    var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
                    _uof.GetRepository().DeleteAsync(employee);
                    await _uof.Commit();
                    return Ok(AppMessages.DELETED);
                }
            }
        }



        [HttpDelete("FireEmployee")]
        public async Task<ActionResult> FireEmployeeAsync(int id)
        {
            var result = await _uof.GetRepository().Exists(id);
            if (!result)
            {
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
            }
            var employee = await _uof.GetRepository().GetByIdAsync(id);
            _uof.GetRepository().DeleteAsync(employee);

            var AttendanceRecords = await _attendanceUof.GetRepository().GetAllWithWhere(A => A.EmpId == employee.Id);
            if (AttendanceRecords != null)
                _attendanceUof.GetRepository().DeleteRangeAsync(AttendanceRecords);

            var PayrollRecords = await _payrollUof.GetRepository().GetAllWithWhere(P => P.EmpId == employee.Id);
            if (PayrollRecords != null)
                _payrollUof.GetRepository().DeleteRangeAsync(PayrollRecords);

            var VacationRecords = await _vacationUof.GetRepository().GetAllWithWhere(v => v.EmpId == employee.Id);
            if (VacationRecords != null)
                _vacationUof.GetRepository().DeleteRangeAsync(VacationRecords);

            await _uof.Commit();
            return Ok(AppMessages.FIRED);
        }
        #endregion
        */
    }
}
