using Application.DTOs.Publisher;
using Application.DTOs.Vacation;
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
using Infrastructure.Specifications.VacationSpec;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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

        #region Get
        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDto>>> GetAllVacationsAsync()
        {
            var vacations = await _uof.GetRepository<Vacation>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Vacation>, IReadOnlyList<ReadVacationDto>>(vacations));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadVacationDto>>> GetAllVacationsWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400));

            var spec = new VacationWithEmployeeSpec(pagesize, pageindex, isPagingEnabled);
            var totalVacation = await _uof.GetRepository<Vacation>().CountAsync(spec);
            if (totalVacation == 0)
                return NotFound(new ApiResponse(404));
            var vacations = await _uof.GetRepository<Vacation>().FindAllSpec(spec);
            var mappedVacations = _mapper.Map<IReadOnlyList<ReadVacationDto>>(vacations);
            var paginationData = new Pagination<ReadVacationDto>(spec.PageIndex, spec.PageSize, totalVacation, mappedVacations);
            return Ok(paginationData);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadVacationDto>> GetVacationByIdAsync(string id)
        {
            if (await _uof.GetRepository<Vacation>().Exists(int.Parse(id)))
            {
                var vacations = await _uof.GetRepository<Vacation>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadVacationDto>> GetVacationByIdWithDetailAsync(string id)
        {
            if (await _uof.GetRepository<Vacation>().Exists(int.Parse(id)))
            {
                var spec = new VacationWithEmployeeSpec(int.Parse(id));
                var vacations = await _uof.GetRepository<Vacation>().FindSpec(spec);
                if (vacations == null)
                    return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<Vacation, ReadVacationDto>(vacations));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadVacationDto>>> SearchVacationWithCriteria(string? empName = null)
        {
            var result = await _vacServ.SearchVactionDataWithDetail(empName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetVacationsCountDto>>> GetTotalVacations(string empId, DateTime FromDate, DateTime ToDate)
        {
            var result = await _vacServ.GetTotalVacationsByEmpId(int.Parse(empId), FromDate, ToDate);
            if (result == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region Post
        [Authorize(Roles = "Manager,HR")]
        [HttpPost]
        public async Task<ActionResult> InsertVacationAsync(CreateVacationDto vacationDto)
        {
            var result = await _uof.GetRepository<Employee>().Exists(vacationDto.EmpId);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_vacServ.IsValidNormalVacation(vacationDto.NormalVacation))
            {
                return BadRequest(new ApiResponse(400, AppMessages.MAX_NORMAL_VACATIONS));
            }
            if (!_vacServ.IsValidUrgentVacation(vacationDto.UrgentVacation))
            {
                vacationDto.UrgentVacation = false;
                vacationDto.Absence = true;
            }
            var vacations = _mapper.Map<CreateVacationDto, Vacation>(vacationDto);
            _uof.GetRepository<Vacation>().InsertAsync(vacations);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region Put
        [Authorize(Roles = "Manager,HR")]
        [HttpPut]
        public async Task<ActionResult> UpdateVacationAsync(ReadVacationDto vacationDto)
        {
            var result = await _uof.GetRepository<Vacation>().Exists(vacationDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_vacServ.IsValidNormalVacation(vacationDto.NormalVacation))
            {
                return BadRequest(new ApiResponse(400, AppMessages.MAX_NORMAL_VACATIONS));
            }
            if (!_vacServ.IsValidUrgentVacation(vacationDto.UrgentVacation))
            {
                vacationDto.UrgentVacation = false;
                vacationDto.Absence = true;
            }
            var vacations = _mapper.Map<ReadVacationDto, Vacation>(vacationDto);
            _uof.GetRepository<Vacation>().UpdateAsync(vacations);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Manager,HR")]
        [HttpDelete]
        public async Task<ActionResult> DeleteVacationAsync(string id)
        {
            var vacationSpec = new VacationWithEmployeeSpec(int.Parse(id));
            var vacation = _uof.GetRepository<Vacation>().FindSpec(vacationSpec).Result;
            if (vacation == null)
                return NotFound(new ApiResponse(404));
            _uof.GetRepository<Vacation>().DeleteAsync(vacation);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
    }
}
