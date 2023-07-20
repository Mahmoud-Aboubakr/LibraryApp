using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPayrollServices _searchPayrollDataWithDetailService;
        private readonly ILogger<PayrollsController> _logger;

        public PayrollsController(IUnitOfWork uof,
                                  IMapper mapper, 
                                  IPayrollServices searchPayrollDataWithDetailService,
                                   ILogger<PayrollsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _searchPayrollDataWithDetailService = searchPayrollDataWithDetailService;
            _logger = logger;
        }

        #region Get
        [HttpGet("GetAllPayrollsAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadPayrollDto>>> GetAllPayrollsAsync()
        {
            var payrolls = await _uof.GetRepository<Payroll>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Payroll>, IReadOnlyList<ReadPayrollDto>>(payrolls));
        }

        [HttpGet("GetAllPayrollsWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadPayrollDetailsDto>>> GetAllPayrollsWithDetails()
        {
            var payrolls = await _uof.GetRepository<Payroll>().GetAllListWithIncludesAsync(new Expression<Func<Payroll, object>>[] { x => x.Employee });
            return Ok(_mapper.Map<IReadOnlyList<Payroll>, IReadOnlyList<ReadPayrollDetailsDto>>(payrolls));
        }

        [HttpGet("GetPayrollById")]
        public async Task<ActionResult<ReadPayrollDto>> GetPayrollByIdAsync(int id)
        {
            if (await _uof.GetRepository<Payroll>().Exists(id))
            {
                var payrolls = await _uof.GetRepository<Payroll>().GetByIdAsync(id);
                return Ok(_mapper.Map<Payroll, ReadPayrollDto>(payrolls));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetPayrollByIdWithDetailAsync")]
        public async Task<ActionResult<ReadPayrollDetailsDto>> GetPayrollByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Payroll>().Exists(id))
            {
                var payrolls = await _uof.GetRepository<Payroll>().GetByIdAsyncWithIncludes(id, new Expression<Func<Payroll, object>>[] { x => x.Employee });
                return Ok(_mapper.Map<Payroll, ReadPayrollDetailsDto>(payrolls));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchPayrollWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadPayrollDetailsDto>>> SearchPayrollWithCriteria(string? empName = null)
        {
            var result = await _searchPayrollDataWithDetailService.SearchPayrollDataWithDetail(empName);
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost("InsertPayroll")]
        public async Task<ActionResult> InsertPayrollAsync(CreatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(payrollDto.EmpId);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {payrollDto.EmpId}" });
            //To get BasicSalary from Employee
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<CreatePayrollDto, Payroll>(payrollDto);
            payrolls.BasicSalary = employee.EmpBasicSalary;
            _uof.GetRepository<Payroll>().InsertAsync(payrolls);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region Put
        [HttpPut("UpdatePayroll")]
        public async Task<ActionResult> UpdatePayrollAsync(UpdatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Payroll>().Exists(payrollDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {payrollDto.Id}" });
            //To get BasicSalary from Employee
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<UpdatePayrollDto, Payroll>(payrollDto);
            payrolls.BasicSalary = employee.EmpBasicSalary;
            _uof.GetRepository<Payroll>().UpdateAsync(payrolls);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region Delete
        [HttpDelete("DeletePayroll")]
        public async Task<ActionResult> DeletePayrollAsync(ReadPayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Payroll>().Exists(payrollDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {payrollDto.Id}" });
            var payrolls = _mapper.Map<ReadPayrollDto, Payroll>(payrollDto);
            _uof.GetRepository<Payroll>().DeleteAsync(payrolls);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
