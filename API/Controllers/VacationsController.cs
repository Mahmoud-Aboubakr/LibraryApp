using Application.DTOs.Vacation;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.AppServices;
using Infrastructure.Specifications.VacationSpec;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly IUnitOfWork<Vacation> _uof;
        private readonly IUnitOfWork<Employee> _employeeUof;
        private readonly IMapper _mapper;
        private readonly IVacationServices _vacServ;
        private readonly ILogger<VacationsController> _logger;

        public VacationsController(IUnitOfWork<Vacation> uof,
                                  IUnitOfWork<Employee> employeeUof,
                                    IMapper mapper,
                                    IVacationServices vacServ,
                                    ILogger<VacationsController> logger)
        {
            _uof = uof;
            _employeeUof = employeeUof;
            _mapper = mapper;
            _vacServ = vacServ;
            _logger = logger;
        }

        #region Get
        [HttpGet("GetAllVacationsAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDto>>> GetAllVacationsAsync()
        {
            var vacations = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Vacation>, IReadOnlyList<ReadVacationDto>>(vacations));
        }

        [HttpGet("GetAllVacationsWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadVacationDto>>> GetAllVacationsWithDetails()
        {
            var spec = new VacationWithEmployeeSpec();
            var vacations = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<Vacation>, IEnumerable<ReadVacationDto>>(vacations));
        }

        [HttpGet("GetVacationById")]
        public async Task<ActionResult<ReadVacationDto>> GetVacationByIdAsync(int id)
        {
            if (await _uof.GetRepository().Exists(id))
            {
                var vacations = await _uof.GetRepository().GetByIdAsync(id);
                return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetVacationByIdWithDetailAsync")]
        public async Task<ActionResult<ReadVacationDto>> GetVacationByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository().Exists(id))
            {
                var spec = new VacationWithEmployeeSpec(id);
                var vacations = await _uof.GetRepository().FindSpec(spec);
                return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchVacationWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDto>>> SearchVacationWithCriteria(string? empName = null)
        {
            var result = await _vacServ.SearchVactionDataWithDetail(empName);
            return Ok(result);
        }

        [HttpGet("GetTotalVacations")]
        public async Task<ActionResult<IReadOnlyList<GetVacationsCountDto>>> GetTotalVacations(int empId, DateTime FromDate, DateTime ToDate)
        {
            var result = await _vacServ.GetTotalVacationsByEmpId(empId, FromDate, ToDate);
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost("InsertVacation")]
        public async Task<ActionResult> InsertVacationAsync(CreateVacationDto vacationDto)
        {
            var result = await _employeeUof.GetRepository().Exists(vacationDto.EmpId);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {vacationDto.EmpId}" });
            if (!_vacServ.IsValidNormalVacation(vacationDto.NormalVacation))
            {
                return BadRequest(new { Detail = AppMessages.MAX_NORMAL_VACATIONS });
            }
            if (!_vacServ.IsValidUrgentVacation(vacationDto.UrgentVacation))
            {
                vacationDto.UrgentVacation = false;
                vacationDto.Absence = true;
            }
            var vacationss = _mapper.Map<CreateVacationDto, Vacation>(vacationDto);
            _uof.GetRepository().InsertAsync(vacationss);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region Put
        [HttpPut("UpdateVacation")]
        public async Task<ActionResult> UpdateVacationAsync(ReadVacationDto vacationDto)
        {
            var result = await _uof.GetRepository().Exists(vacationDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {vacationDto.Id}" });
            var vacations = _mapper.Map<ReadVacationDto, Vacation>(vacationDto);
            _uof.GetRepository().UpdateAsync(vacations);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteVacation")]
        public async Task<ActionResult> DeleteVacationAsync(ReadVacationDto vacationDto)
        {
            var vacations = _mapper.Map<ReadVacationDto, Vacation>(vacationDto);
            _uof.GetRepository().DeleteAsync(vacations);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
        
    }
}
