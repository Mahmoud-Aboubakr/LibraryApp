using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IAttendencePermissionValidator _attendencePermissionValidator;
        private readonly IAttendenceMonthValidator _attendenceMonthValidator;
        public readonly ISearchAttendenceDataWithDetailService _searchAttendenceDataWithDetailService;
        private readonly ILogger<AttendenceController> _logger;

        public AttendenceController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IAttendencePermissionValidator attendencePermissionValidator,
                                    IAttendenceMonthValidator attendenceMonthValidator,
                                    ISearchAttendenceDataWithDetailService searchAttendenceDataWithDetailService,
                                    ILogger<AttendenceController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _attendencePermissionValidator = attendencePermissionValidator;
            _attendenceMonthValidator = attendenceMonthValidator;
            _searchAttendenceDataWithDetailService = searchAttendenceDataWithDetailService;
            _logger = logger;
        }

        [HttpGet("GetAllAttendenceAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> GetAllAttendenceAsync()
        {
            var attendences = await _uof.GetRepository<Attendence>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Attendence>, IReadOnlyList<ReadAttendanceDto>>(attendences));
        }


        [HttpGet("GetAllAttendenceWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadAttendenceDetailsDto>>> GetAllAttendenceWithDetails()
        {
            var attendences = await _uof.GetRepository<Attendence>().GetAllListWithIncludesAsync(new Expression<Func<Attendence, object>>[] { x => x.Employee });
            return Ok(_mapper.Map<IReadOnlyList<Attendence>, IReadOnlyList<ReadAttendenceDetailsDto>>(attendences));
        }

        [HttpGet("GetAttendenceById")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdAsync(int id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(id))
            {
                var attendences = await _uof.GetRepository<Attendence>().GetByIdAsync(id);
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpGet("GetAttendenceByIdWithDetailAsync")]
        public async Task<ActionResult<ReadAttendenceDetailsDto>> GetAttendenceByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(id))
            {
                var attendences = await _uof.GetRepository<Attendence>().GetByIdAsyncWithIncludes(id, new Expression<Func<Attendence, object>>[] { x => x.Employee });
                return Ok(_mapper.Map<Attendence, ReadAttendenceDetailsDto>(attendences));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpPost("InsertAttendence")]
        public async Task<ActionResult> InsertAttendenceAsync(CreateAttendenceDto attendenceDto)
        {
            if (!_attendencePermissionValidator.IsValidAttendencePermission(attendenceDto.Permission))
                return BadRequest(new { Detail = $"This is invalid input (Permission) {attendenceDto.Permission}" });
            if (!_attendenceMonthValidator.IsValidMonth(attendenceDto.Month))
                return BadRequest(new { Detail = $"This is invalid month value {attendenceDto.Month}" });
            var result = await _uof.GetRepository<Employee>().Exists(attendenceDto.EmpId);
            if (!result)
                    return BadRequest(new { Detail = $"This is no employee to add attendence {attendenceDto.EmpId}" });
            var attendences = _mapper.Map<CreateAttendenceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().InsertAsync(attendences);
            await _uof.Commit();

            return Ok(_mapper.Map<Attendence, CreateAttendenceDto>(attendences));
        }

    
        [HttpPut("UpdateAttendence")]
        public async Task<ActionResult> UpdateAttendenceAsync(ReadAttendanceDto attendenceDto)
        {
            if (!_attendencePermissionValidator.IsValidAttendencePermission(attendenceDto.Permission))
                return BadRequest(new { Detail = $"This is invalid input (Permission) {attendenceDto.Permission}" });
            if (!_attendenceMonthValidator.IsValidMonth(attendenceDto.Month))
                return BadRequest(new { Detail = $"This is invalid month value {attendenceDto.Month}" });
            var result = await _uof.GetRepository<Attendence>().Exists(attendenceDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no attendence record to update it {attendenceDto.Id}" });
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().UpdateAsync(attendences);
            await _uof.Commit();

            return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
        }


        [HttpDelete("DeleteAttendence")]
        public async Task<ActionResult> DeleteAttendenceAsync(ReadAttendanceDto attendenceDto)
        {
            var result = await _uof.GetRepository<Attendence>().Exists(attendenceDto.Id);
            if (!result)
                return BadRequest(new { Detail = $"This is no attendence record to delete it {attendenceDto.Id}" });
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().DeleteAsync(attendences);
            await _uof.Commit();
            return Ok();
        }


        [HttpGet("SearchEmployeeWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadAttendenceDetailsDto>>> SearchEmpWithCriteria(string? empName = null)
        {
            var result = await _searchAttendenceDataWithDetailService.SearchAttendenceDataWithDetail(empName);
            return Ok(result);
        }
    }
}
