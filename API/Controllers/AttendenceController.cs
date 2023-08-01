using Application.DTOs.Attendance;
using Application.DTOs.Book;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
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
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400));

            var spec = new AttendanceWithEmployeeSpec(pagesize, pageindex, isPagingEnabled);

            var totalAttendences = await _uof.GetRepository<Attendence>().CountAsync(spec);
            if(totalAttendences == 0)
                return NotFound(new ApiResponse(404));

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

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [HttpGet("GetAttendenceByIdWithDetailAsync")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdWithDetailAsync(int id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(id))
            {
                var spec = new AttendanceWithEmployeeSpec(id);
                var attendences = await _uof.GetRepository<Attendence>().FindSpec(spec);
                if (attendences == null)
                    return NotFound(new ApiResponse(404));
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }


        [HttpGet("SearchAttendenceWithCriteria")]

        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> SearchEmpWithCriteria(string? empName = null)
        {
            var result = await _attendenceServices.SearchAttendenceDataWithDetail(empName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }

        #endregion

        #region POST
        [HttpPost("InsertAttendence")]
        public async Task<ActionResult> InsertAttendenceAsync(CreateAttendenceDto attendenceDto)
        {
            if (!_attendenceServices.IsValidAttendencePermission(attendenceDto.Permission))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PERMISSION));
            if (!_attendenceServices.IsValidPermission(attendenceDto.Permission))
                attendenceDto.Permission = 2;
            var result = await _uof.GetRepository<Employee>().Exists(attendenceDto.EmpId);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
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
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PERMISSION));
            if (!_attendenceServices.IsValidPermission(attendenceDto.Permission))
                attendenceDto.Permission = 2;
            var result = await _uof.GetRepository<Attendence>().Exists(attendenceDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
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
            if (attendence == null)
                return NotFound(new ApiResponse(404));
            _uof.GetRepository<Attendence>().DeleteAsync(attendence);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
