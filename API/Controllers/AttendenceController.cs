using Application.DTOs.Attendance;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Specifications.AttendanceSpec;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IUnitOfWork<Attendence> _uof;
        private readonly IUnitOfWork<Employee> _employeeUof;
        private readonly IMapper _mapper;
        private readonly IAttendenceServices _attendenceServices;
        private readonly ILogger<AttendenceController> _logger;

        public AttendenceController(IUnitOfWork<Attendence> uof,
                                    IUnitOfWork<Employee> employeeUof,
                                    IMapper mapper,
                                    IAttendenceServices attendenceServices,
                                    ILogger<AttendenceController> logger)
        {
            _uof = uof;
            _employeeUof = employeeUof;
            _mapper = mapper;
            _attendenceServices = attendenceServices;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllAttendenceAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> GetAllAttendenceAsync()
        {
            var attendences = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Attendence>, IReadOnlyList<ReadAttendanceDto>>(attendences));
        }


        [HttpGet("GetAllAttendenceWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadAttendanceDto>>> GetAllAttendenceWithDetails()
        {
            var spec = new AttendanceWithEmployeeSpec();
            var attendences = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<Attendence>, IEnumerable<ReadAttendanceDto>>(attendences));
        }

        [HttpGet("GetAttendenceById")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdAsync(int id)
        {
            if (await _uof.GetRepository().Exists(id))
            {
                var attendences = await _uof.GetRepository().GetByIdAsync(id);
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return NotFound(new { Detail = AppMessages.INVALID_ID });
        }

        [HttpGet("GetAttendenceByIdWithDetailAsync")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository().Exists(id))
            {
                var spec = new AttendanceWithEmployeeSpec(id);
                var attendences = await _uof.GetRepository().FindSpec(spec);
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return NotFound(new { Detail = AppMessages.INVALID_ID });
        }


        [HttpGet("SearchEmployeeWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> SearchEmpWithCriteria(string? empName = null)
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
            var result = await _employeeUof.GetRepository().Exists(attendenceDto.EmpId);
            if (!result)
                return BadRequest(new { Detail = $"{AppMessages.INVALID_ID} {attendenceDto.EmpId}" });
            var attendences = _mapper.Map<CreateAttendenceDto, Attendence>(attendenceDto);
            _uof.GetRepository().InsertAsync(attendences);
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
            var result = await _uof.GetRepository().Exists(attendenceDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {attendenceDto.Id}" });
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository().UpdateAsync(attendences);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteAttendence")]
        public async Task<ActionResult> DeleteAttendenceAsync(ReadAttendanceDto attendenceDto)
        {
            var attendences = _mapper.Map<ReadAttendanceDto, Attendence>(attendenceDto);
            _uof.GetRepository().DeleteAsync(attendences);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
        
    }
}
