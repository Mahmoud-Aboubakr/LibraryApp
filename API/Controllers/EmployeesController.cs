using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServices;
using Infrastructure.AppServicesContracts;
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
        private readonly IEmployeeTypeValidator _employeeTypeValidator;
        private readonly IEmployeeAgeValidator _employeeAgeValidator;
        private readonly ISearchEmployeeDataService _searchEmployeeDataService;
        private readonly ILogger<EmployeesController> _logger;
        
        public EmployeesController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IPhoneNumberValidator phoneNumberValidator,
                                    IEmployeeTypeValidator employeeTypeValidator,
                                    IEmployeeAgeValidator employeeAgeValidator,
                                    ISearchEmployeeDataService searchEmployeeDataService,
                                    ILogger<EmployeesController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _employeeTypeValidator = employeeTypeValidator;
            _employeeAgeValidator = employeeAgeValidator;
            _searchEmployeeDataService = searchEmployeeDataService;
            _logger = logger;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> GetAllEmployeesAsync()
        {
            var employees = await _uof.GetRepository<Employee>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<ReadEmployeeDto>>(employees));
        }


        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult> GetEmployeeByIdAsync(int id)
        {
            if (await _uof.GetRepository<Employee>().Exists(id))
            {
                var employee = await _uof.GetRepository<Employee>().GetByIdAsync(id);
                return Ok(_mapper.Map<Employee, ReadEmployeeDto>(employee));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }


        [HttpPost("InsertEmployee")]
        public async Task<ActionResult> InsertEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            if (!_employeeTypeValidator.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new { Detail = $"This is invalid Employee Type {employeeDto.EmpType}" });
            if (!_employeeAgeValidator.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"This is invalid Employee Age {employeeDto.EmpAge}" });
            if(!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"This is invalid phone number {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().InsertAsync(employee);
            await _uof.Commit();

            return Ok(_mapper.Map<Employee, CreateEmployeeDto>(employee));
        }


        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(employeeDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"Can't update employee not exists before {employeeDto.Id}" });
            if (!_employeeTypeValidator.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new { Detail = $"This is invalid Employee Type {employeeDto.EmpType}" });
            if (!_employeeAgeValidator.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"This is invalid Employee Age {employeeDto.EmpAge}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"This is invalid phone number {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().UpdateAsync(employee);
            await _uof.Commit();

            return Ok(_mapper.Map<Employee, ReadEmployeeDto>(employee));
        }


        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var UsedInAttendance = await _uof.GetRepository<Attendence>().FindUsingWhereAsync(x => x.EmpId == employeeDto.Id);
            var UsedInPayroll = await _uof.GetRepository<Payroll>().FindUsingWhereAsync(p => p.EmpId == employeeDto.Id);
            var UsedInVacation = await _uof.GetRepository<Vacation>().FindUsingWhereAsync(v => v.EmpId == employeeDto.Id);

            var result = await _uof.GetRepository<Employee>().Exists(employeeDto.Id);
            if (!result)
            {
                return BadRequest(new { Detail = $"Can't delete employee not exists before {employeeDto.Id}" });
            }
            else
            {
                if(UsedInAttendance || UsedInPayroll || UsedInVacation)
                {
                    return BadRequest(new { Detail = $"Can't delete this employee because it exixts in onther tables {employeeDto.Id}" });
                }
                else
                {
                    var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
                    _uof.GetRepository<Employee>().DeleteAsync(employee);
                    await _uof.Commit();
                    return Ok();
                }
            }
        }


        [HttpGet("SearchEmployeeWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadEmployeeDto>>> SearchWithCriteria(string name = null, byte? type = null, string phone = null, decimal? salary = null)
        {
            var result = await _searchEmployeeDataService.SearchEmployeeDataWithDetail(name, type, phone, salary);
            return Ok(result);
        }


        [HttpDelete("FireEmployee")]
        public async Task<ActionResult> FireEmployeeAsync(int id)
        {
            var result = await _uof.GetRepository<Employee>().Exists(id);
            if (!result)
            {
                return BadRequest(new { Detail = $"Not Found!" });
            }
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(id);
            _uof.GetRepository<Employee>().DeleteAsync(employee);

            var AttendanceRecords = await _uof.GetRepository<Attendence>().GetAllWithWhere(A => A.EmpId == employee.Id);
            if(AttendanceRecords != null)
            _uof.GetRepository<Attendence>().DeleteRangeAsync(AttendanceRecords);

            var PayrollRecords = await _uof.GetRepository<Payroll>().GetAllWithWhere(P => P.EmpId == employee.Id);
            if(PayrollRecords != null)
            _uof.GetRepository<Payroll>().DeleteRangeAsync(PayrollRecords);

            var VacationRecords = await _uof.GetRepository<Vacation>().GetAllWithWhere(v => v.EmpId == employee.Id);
            if (VacationRecords != null)
            _uof.GetRepository<Vacation>().DeleteRangeAsync(VacationRecords);

            await _uof.Commit();
            return Ok();
        }
    }
}
