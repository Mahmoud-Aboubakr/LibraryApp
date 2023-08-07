using Application.DTOs.Attendance;
using Application.DTOs.Payroll;
using Application.DTOs.Publisher;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.AppServices;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.PayrollSpec;
using Infrastructure.Specifications.PublisherSpec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PayrollsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPayrollServices _payrollServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly IAttendenceServices _attendenceServices;
        private readonly IVacationServices _vacationServices;
        private readonly ILogger<PayrollsController> _logger;

        public PayrollsController(IUnitOfWork uof,
                                  IMapper mapper, 
                                  IPayrollServices payrollServices,
                                  IEmployeeServices employeeServices,
                                  IAttendenceServices attendenceServices,
                                  IVacationServices vacationServices,
                                  ILogger<PayrollsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _payrollServices = payrollServices;
            _employeeServices = employeeServices;
            _attendenceServices = attendenceServices;
            _vacationServices = vacationServices;
            _logger = logger;
        }
        [Authorize(Roles = "Manager,HR")]
        #region Get
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadPayrollDto>>> GetAllPayrollsAsync()
        {
            var payrolls = await _uof.GetRepository<Payroll>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Payroll>, IReadOnlyList<ReadPayrollDto>>(payrolls));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadPayrollDto>>> GetAllPayrollsWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400));

            var spec = new PayrollWithEmployeeSpec(pagesize, pageindex, isPagingEnabled);
            var totalPayrolls = await _uof.GetRepository<Payroll>().CountAsync(spec);
            if (totalPayrolls == 0)
                return NotFound(new ApiResponse(404));
            var payrolls = await _uof.GetRepository<Payroll>().FindAllSpec(spec);
            var mappedPayrolls = _mapper.Map<IReadOnlyList<ReadPayrollDto>>(payrolls);
            var paginationData = new Pagination<ReadPayrollDto>(spec.PageIndex, spec.PageSize, totalPayrolls, mappedPayrolls);
            return Ok(paginationData);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadPayrollDto>> GetPayrollByIdAsync(string id)
        {
            if (await _uof.GetRepository<Payroll>().Exists(int.Parse(id)))
            {
                var payrolls = await _uof.GetRepository<Payroll>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Payroll, ReadPayrollDto>(payrolls));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadPayrollDto>> GetPayrollByIdWithDetailAsync(string id)
        {
            if (await _uof.GetRepository<Payroll>().Exists(int.Parse(id)))
            {
                var spec = new PayrollWithEmployeeSpec(int.Parse(id));
                var payrolls = await _uof.GetRepository<Payroll>().FindSpec(spec);
                if (payrolls == null)
                    return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<Payroll, ReadPayrollDto>(payrolls));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadPayrollDto>>> SearchPayrollWithCriteria(string? empName = null)
        {
            var result = await _payrollServices.SearchPayrollDataWithDetail(empName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region Post
        [Authorize(Roles = "Manager,HR")]
        [HttpPost]
        public async Task<ActionResult> InsertPayrollAsync(CreatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(payrollDto.EmpId);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));

            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<CreatePayrollDto, Payroll>(payrollDto);

            var DailyPay = _employeeServices.CalculateDailyPay(payrolls.EmpId).Result;
            DailyPay = Math.Round(DailyPay, 2);
            var HourlyPay = _employeeServices.CalculateHourlyPay(payrolls.EmpId).Result;
            HourlyPay = Math.Round(HourlyPay, 2);

            var AbsenceDays = _vacationServices.GetAbsenceDaysByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;
            var LateHours = _attendenceServices.GetLateHoursByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;

            decimal deduct = _payrollServices.CalculateDeduct(AbsenceDays, DailyPay, LateHours, HourlyPay);

            payrolls.Deduct += deduct;

            var ExtraHours = _attendenceServices.GetExtraHoursByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;

            decimal bonus = _payrollServices.CalculateBonus(ExtraHours, HourlyPay);

            payrolls.Bonus += bonus;

            payrolls.BasicSalary = employee.EmpBasicSalary;

            payrolls.SalaryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);

            _uof.GetRepository<Payroll>().InsertAsync(payrolls);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region Put
        [Authorize(Roles = "Manager,HR")]
        [HttpPut]
        public async Task<ActionResult> UpdatePayrollAsync(UpdatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Payroll>().Exists(payrollDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));

            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<UpdatePayrollDto, Payroll>(payrollDto);

            var DailyPay = _employeeServices.CalculateDailyPay(payrolls.EmpId).Result;
            DailyPay = Math.Round(DailyPay, 2);
            var HourlyPay = _employeeServices.CalculateHourlyPay(payrolls.EmpId).Result;
            HourlyPay = Math.Round(HourlyPay, 2);

            var AbsenceDays = _vacationServices.GetAbsenceDaysByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;
            var LateHours = _attendenceServices.GetLateHoursByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;

            decimal deduct = _payrollServices.CalculateDeduct(AbsenceDays, DailyPay, LateHours, HourlyPay);

            payrolls.Deduct += deduct;

            var ExtraHours = _attendenceServices.GetExtraHoursByMonth(payrolls.EmpId, payrolls.SalaryDate.Month).Result;

            decimal bonus = _payrollServices.CalculateBonus(ExtraHours, HourlyPay);

            payrolls.Bonus += bonus;

            payrolls.BasicSalary = employee.EmpBasicSalary;

            payrolls.SalaryDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);

            _uof.GetRepository<Payroll>().UpdateAsync(payrolls);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }

        #endregion

        #region Delete
        [Authorize(Roles = "Manager,HR")]
        [HttpDelete]
        public async Task<ActionResult> DeletePayrollAsync(string id)
        {
            var payrollSpec = new PayrollWithEmployeeSpec(int.Parse(id));
            var payroll = _uof.GetRepository<Payroll>().FindSpec(payrollSpec).Result;
            if (payroll == null)
                return NotFound(new ApiResponse(404));
            _uof.GetRepository<Payroll>().DeleteAsync(payroll);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
