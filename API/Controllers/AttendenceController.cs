using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
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
        private readonly IAttendenceServices _attendenceServices;
        private readonly ILogger<AttendenceController> _logger;

        public AttendenceController(IUnitOfWork uof,
                                    IMapper mapper,
                                    IAttendenceServices attendenceServices,
                                    ILogger<AttendenceController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _attendenceServices = attendenceServices;
            _logger = logger;
        }

        #region GET
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

            return NotFound(new { Detail = AppMessages.INVALID_ID });
        }

        [HttpGet("GetAttendenceByIdWithDetailAsync")]
        public async Task<ActionResult<ReadAttendenceDetailsDto>> GetAttendenceByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(id))
            {
                var attendences = await _uof.GetRepository<Attendence>().GetByIdAsyncWithIncludes(id, new Expression<Func<Attendence, object>>[] { x => x.Employee });
                return Ok(_mapper.Map<Attendence, ReadAttendenceDetailsDto>(attendences));
            }

            return NotFound(new { Detail = AppMessages.INVALID_ID });
        }


        [HttpGet("SearchEmployeeWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadAttendenceDetailsDto>>> SearchEmpWithCriteria(string? empName = null)
        {
            var result = await _attendenceServices.SearchAttendenceDataWithDetail(empName);
            return Ok(result);
        }

        #endregion

        #region POST
        [HttpPost("InsertAttendence")]
        public async Task<ActionResult> InsertAttendenceAsync(CreateAttendenceDto attendenceDto)
        {
            if (!_attendenceServices.IsValidAttendencePermission(attendenceDto.Permission))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PERMISSION} {attendenceDto.Permission}" });
            if (!_attendenceServices.IsValidMonth(attendenceDto.Month))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_MONTH} {attendenceDto.Month}" });
            var result = await _uof.GetRepository<Employee>().Exists(attendenceDto.EmpId);
            if (!result)
                return BadRequest(new { Detail = $"{AppMessages.INVALID_ID} {attendenceDto.EmpId}" });
            var attendences = _mapper.Map<CreateAttendenceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().InsertAsync(attendences);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region PUT
        [HttpPut("UpdateAttendence")]
        public async Task<ActionResult> UpdateAttendenceAsync(ReadAttendanceDto attendenceDto)
        {
            if (!_attendenceServices.IsValidAttendencePermission(attendenceDto.Permission))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PERMISSION} {attendenceDto.Permission}" });
            if (!_attendenceServices.IsValidMonth(attendenceDto.Month))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_MONTH} {attendenceDto.Month}" });
            var result = await _uof.GetRepository<Attendence>().Exists(attendenceDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {attendenceDto.Id}" });
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().UpdateAsync(attendences);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteAttendence")]
        public async Task<ActionResult> DeleteAttendenceAsync(ReadAttendanceDto attendenceDto)
        {
            var result = await _uof.GetRepository<Attendence>().Exists(attendenceDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {attendenceDto.Id}" });
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository<Attendence>().DeleteAsync(attendences);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
