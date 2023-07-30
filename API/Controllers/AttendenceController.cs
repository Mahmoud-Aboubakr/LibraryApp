using Application.DTOs.Attendance;
using Application.DTOs.Book;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BookSpec;
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
        public async Task<ActionResult<Pagination<ReadAttendanceDto>>> GetAllAttendenceWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new AttendanceWithEmployeeSpec(pagesize, pageindex, isPagingEnabled);

            var totalAttendences = await _uof.GetRepository<Attendence>().CountAsync(spec);

            var attendences = await _uof.GetRepository<Attendence>().FindAllSpec(spec);

            var mappedattendences = _mapper.Map<IReadOnlyList<ReadAttendanceDto>>(attendences);

            var paginationData = new Pagination<ReadAttendanceDto>(spec.PageIndex, spec.PageSize, totalAttendences, mappedattendences);

            return Ok(paginationData);
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
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(id))
            {
                var spec = new AttendanceWithEmployeeSpec(id);
                var attendences = await _uof.GetRepository<Attendence>().FindSpec(spec);
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
            if (!_attendenceServices.IsValidPermission(attendenceDto.Permission))
                attendenceDto.Permission = 2;
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
            if (!_attendenceServices.IsValidPermission(attendenceDto.Permission))
                attendenceDto.Permission = 2;
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
        public async Task<ActionResult> DeleteAttendenceAsync(int id)
        {
            var attendenceSpec = new AttendanceWithEmployeeSpec(id);
            var attendence = _uof.GetRepository<Attendence>().FindSpec(attendenceSpec).Result;
            _uof.GetRepository<Attendence>().DeleteAsync(attendence);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
