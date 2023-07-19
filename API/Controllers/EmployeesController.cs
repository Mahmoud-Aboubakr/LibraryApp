﻿using Application.DTOs;
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
        private readonly EmployeeServices _employeeServices;
        private readonly ILogger<EmployeesController> _logger;
        
        public EmployeesController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IPhoneNumberValidator phoneNumberValidator,
                                    EmployeeServices employeeServices,
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

            return NotFound(new { Detail = $"This is invalid Id" });
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
                return BadRequest(new { Detail = $"This is invalid Employee Type {employeeDto.EmpType}" });
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"This is invalid Employee Age {employeeDto.EmpAge}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"This is invalid phone number {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<CreateEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().InsertAsync(employee);
            await _uof.Commit();

            return StatusCode(201, "Employee Inserted Successfully"); 
        }
        #endregion

        #region PUT
        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(employeeDto.Id);
            if (!result)
                return NotFound(new { Detail = $"Can't update employee not exists before {employeeDto.Id}" });
            if (!_employeeServices.IsValidEmployeeType(employeeDto.EmpType))
                return BadRequest(new { Detail = $"This is invalid Employee Type {employeeDto.EmpType}" });
            if (!_employeeServices.IsValidEmployeeAge(employeeDto.EmpAge))
                return BadRequest(new { Detail = $"This is invalid Employee Age {employeeDto.EmpAge}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(employeeDto.EmpPhoneNumber))
                return BadRequest(new { Detail = $"This is invalid phone number {employeeDto.EmpPhoneNumber}" });
            var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
            _uof.GetRepository<Employee>().UpdateAsync(employee);
            await _uof.Commit();

            return Ok("Updated Successfully");
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployeeAsync(ReadEmployeeDto employeeDto)
        {
            var UsedInAttendance = await _uof.GetRepository<Attendence>().FindUsingWhereAsync(x => x.EmpId == employeeDto.Id);
            var UsedInPayroll = await _uof.GetRepository<Payroll>().FindUsingWhereAsync(p => p.EmpId == employeeDto.Id);
            var UsedInVacation = await _uof.GetRepository<Vacation>().FindUsingWhereAsync(v => v.EmpId == employeeDto.Id);

            var result = await _uof.GetRepository<Employee>().Exists(employeeDto.Id);
            if (!result)
            {
                return NotFound(new { Detail = $"Can't delete employee not exists before {employeeDto.Id}" });
            }
            else
            {
                if (UsedInAttendance || UsedInPayroll || UsedInVacation)
                {
                    return BadRequest(new { Detail = $"Can't delete this employee because it exixts in onther tables {employeeDto.Id}" });
                }
                else
                {
                    var employee = _mapper.Map<ReadEmployeeDto, Employee>(employeeDto);
                    _uof.GetRepository<Employee>().DeleteAsync(employee);
                    await _uof.Commit();
                    return Ok("Deleted Successfully");
                }
            }
        }



        [HttpDelete("FireEmployee")]
        public async Task<ActionResult> FireEmployeeAsync(int id)
        {
            var result = await _uof.GetRepository<Employee>().Exists(id);
            if (!result)
            {
                return NotFound(new { Detail = $"Not Found!" });
            }
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(id);
            _uof.GetRepository<Employee>().DeleteAsync(employee);

            var AttendanceRecords = await _uof.GetRepository<Attendence>().GetAllWithWhere(A => A.EmpId == employee.Id);
            if (AttendanceRecords != null)
                _uof.GetRepository<Attendence>().DeleteRangeAsync(AttendanceRecords);

            var PayrollRecords = await _uof.GetRepository<Payroll>().GetAllWithWhere(P => P.EmpId == employee.Id);
            if (PayrollRecords != null)
                _uof.GetRepository<Payroll>().DeleteRangeAsync(PayrollRecords);

            var VacationRecords = await _uof.GetRepository<Vacation>().GetAllWithWhere(v => v.EmpId == employee.Id);
            if (VacationRecords != null)
                _uof.GetRepository<Vacation>().DeleteRangeAsync(VacationRecords);

            await _uof.Commit();
            return Ok("Fired Successfully");
        }
        #endregion

    }
}
