using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IVacationServices _vacServ;
        private readonly ILogger<VacationsController> _logger;

        public VacationsController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IVacationServices vacServ,
                                    ILogger<VacationsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _vacServ = vacServ;
            _logger = logger;
        }

        [HttpGet("GetAllVacationsAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDto>>> GetAllVacationsAsync()
        {
            var vacations = await _uof.GetRepository<Vacation>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Vacation>, IReadOnlyList<ReadVacationDto>>(vacations));
        }

        [HttpGet("GetAllVacationsWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDetailsDto>>> GetAllVacationsWithDetails()
        {
            var vacations = await _uof.GetRepository<Vacation>().GetAllListWithIncludesAsync(new Expression<Func<Vacation, object>>[] { x => x.Employee });
            return Ok(_mapper.Map<IReadOnlyList<Vacation>, IReadOnlyList<ReadVacationDetailsDto>>(vacations));
        }

        [HttpGet("GetVacationById")]
        public async Task<ActionResult<ReadVacationDto>> GetVacationByIdAsync(int id)
        {
            if (await _uof.GetRepository<Vacation>().Exists(id))
            {
                var vacations = await _uof.GetRepository<Vacation>().GetByIdAsync(id);
                return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpGet("GetVacationByIdWithDetailAsync")]
        public async Task<ActionResult<ReadVacationDetailsDto>> GetVacationByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Vacation>().Exists(id))
            {
                var vacations = await _uof.GetRepository<Vacation>().GetByIdAsyncWithIncludes(id, new Expression<Func<Vacation, object>>[] { x => x.Employee });
                return Ok(_mapper.Map<Vacation, ReadVacationDetailsDto>(vacations));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpPost("InsertVacation")]
        public async Task<ActionResult> InsertVacationAsync(CreateVacationDto vacationDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(vacationDto.EmpId);
            if (!result)
                return BadRequest(new { Detail = $"This is no employee to add vacations {vacationDto.EmpId}" });
            if (!_vacServ.IsValidNormalVacation(vacationDto.NormalVacation))
            {
                return BadRequest(new { Detail = $"You Can't take more normal vacations" });
            }
            if (!_vacServ.IsValidUrgentVacation(vacationDto.UrgentVacation))
            {
                vacationDto.UrgentVacation = false;
                vacationDto.Absence = true;
            }
            var vacationss = _mapper.Map<CreateVacationDto, Vacation>(vacationDto);
            _uof.GetRepository<Vacation>().InsertAsync(vacationss);
            await _uof.Commit();

            return Ok(_mapper.Map<Vacation, CreateVacationDto>(vacationss));
        }

        [HttpPut("UpdateVacation")]
        public async Task<ActionResult> UpdateVacationAsync(ReadVacationDto vacationDto)
        {
            var result = await _uof.GetRepository<Vacation>().Exists(vacationDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no vacation record to update it {vacationDto.Id}" });
            var vacations = _mapper.Map<ReadVacationDto, Vacation>(vacationDto);
            _uof.GetRepository<Vacation>().UpdateAsync(vacations);
            await _uof.Commit();

            return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
        }


        [HttpDelete("DeleteVacation")]
        public async Task<ActionResult> DeleteVacationAsync(ReadVacationDto vacationDto)
        {
            var result = await _uof.GetRepository<Vacation>().Exists(vacationDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no vacation record to delete it {vacationDto.Id}" });
            var vacations = _mapper.Map<ReadVacationDto, Vacation>(vacationDto);
            _uof.GetRepository<Vacation>().DeleteAsync(vacations);
            await _uof.Commit();
            return Ok();
        }

        [HttpGet("SearchVacationWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadVacationDetailsDto>>> SearchVacationWithCriteria(string? empName = null)
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
    }
}
