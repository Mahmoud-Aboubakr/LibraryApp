using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
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
        private readonly ISearchPayrollDataWithDetailService _searchPayrollDataWithDetailService;
        private readonly ILogger<PayrollsController> _logger;

        public PayrollsController(IUnitOfWork uof,
                                  IMapper mapper, 
                                  ISearchPayrollDataWithDetailService searchPayrollDataWithDetailService,
                                   ILogger<PayrollsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _searchPayrollDataWithDetailService = searchPayrollDataWithDetailService;
            _logger = logger;
        }


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

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpGet("GetPayrollByIdWithDetailAsync")]
        public async Task<ActionResult<ReadPayrollDetailsDto>> GetPayrollByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Payroll>().Exists(id))
            {
                var payrolls = await _uof.GetRepository<Payroll>().GetByIdAsyncWithIncludes(id, new Expression<Func<Payroll, object>>[] { x => x.Employee });
                return Ok(_mapper.Map<Payroll, ReadPayrollDetailsDto>(payrolls));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpPost("InsertPayroll")]
        public async Task<ActionResult> InsertPayrollAsync(CreatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(payrollDto.EmpId);
            if (!result)
                return BadRequest(new { Detail = $"This is no employee to add payroll {payrollDto.EmpId}" });
            //To get BasicSalary from Employee
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<CreatePayrollDto, Payroll>(payrollDto);
            payrolls.BasicSalary = employee.EmpBasicSalary;
            _uof.GetRepository<Payroll>().InsertAsync(payrolls);
            await _uof.Commit();

            return Ok(_mapper.Map<Payroll, CreatePayrollDto>(payrolls));
        }

        [HttpPut("UpdatePayroll")]
        public async Task<ActionResult> UpdatePayrollAsync(UpdatePayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Payroll>().Exists(payrollDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no payroll record to update it {payrollDto.Id}" });
            //To get BasicSalary from Employee
            var employee = await _uof.GetRepository<Employee>().GetByIdAsync(payrollDto.EmpId);
            var payrolls = _mapper.Map<UpdatePayrollDto, Payroll>(payrollDto);
            payrolls.BasicSalary = employee.EmpBasicSalary;
            _uof.GetRepository<Payroll>().UpdateAsync(payrolls);
            await _uof.Commit();

            return Ok(_mapper.Map<Payroll, ReadPayrollDto>(payrolls));
        }

        [HttpDelete("DeletePayroll")]
        public async Task<ActionResult> DeletePayrollAsync(ReadPayrollDto payrollDto)
        {
            var result = await _uof.GetRepository<Payroll>().Exists(payrollDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no payroll record to delete it {payrollDto.Id}" });
            var payrolls = _mapper.Map<ReadPayrollDto, Payroll>(payrollDto);
            _uof.GetRepository<Payroll>().DeleteAsync(payrolls);
            await _uof.Commit();
            return Ok();
        }

        [HttpGet("SearchPayrollWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadPayrollDetailsDto>>> SearchPayrollWithCriteria(string? empName = null)
        {
            var result = await _searchPayrollDataWithDetailService.SearchPayrollDataWithDetail(empName);
            return Ok(result);
        }
    }
}
