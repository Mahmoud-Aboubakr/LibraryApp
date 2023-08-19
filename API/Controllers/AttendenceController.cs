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
using Application;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BookSpec;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Application.DTOs.Employee;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> GetAllAttendenceAsync()
        {
            var attendences = await _uof.GetRepository<Attendence>().GetAllAsync();
            if (attendences == null || attendences.Count == 0)
            {
                return NotFound(new ApiResponse(404, AppMessages.NULL_DATA));
            }
            return Ok(_mapper.Map<IReadOnlyList<Attendence>, IReadOnlyList<ReadAttendanceDto>>(attendences));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadAttendanceDto>>> GetAllAttendenceWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));

            var spec = new AttendanceWithEmployeeSpec(pagesize, pageindex, isPagingEnabled);

            var totalAttendences = await _uof.GetRepository<Attendence>().CountAsync(spec);

            var attendences = await _uof.GetRepository<Attendence>().FindAllSpec(spec);

            var mappedattendences = _mapper.Map<IReadOnlyList<ReadAttendanceDto>>(attendences);

            if (mappedattendences == null && totalAttendences == 0)
            {
                return NotFound(new ApiResponse(404, AppMessages.NULL_DATA));
            }

            var paginationData = new Pagination<ReadAttendanceDto>(spec.Skip, spec.Take, totalAttendences, mappedattendences);

            return Ok(paginationData);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdAsync(string id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(int.Parse(id)))
            {
                var attendences = await _uof.GetRepository<Attendence>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadAttendanceDto>> GetAttendenceByIdWithDetailAsync(string id)
        {
            if (await _uof.GetRepository<Attendence>().Exists(int.Parse(id)))
            {
                var spec = new AttendanceWithEmployeeSpec(int.Parse(id));
                var attendences = await _uof.GetRepository<Attendence>().FindSpec(spec);
                if (attendences == null)
                    return NotFound(new ApiResponse(404, AppMessages.NULL_DATA));
                return Ok(_mapper.Map<Attendence, ReadAttendanceDto>(attendences));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,HR")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadAttendanceDto>>> SearchEmpWithCriteria(string? empName = null)
        {
            var result = await _attendenceServices.SearchAttendenceDataWithDetail(empName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404, AppMessages.NOTFOUND_SEARCHDATA));
            }
            return Ok(result);
        }

        #endregion

        #region POST
        [Authorize(Roles = "Manager,HR")]
        [HttpPost]
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

            return Ok(new ApiResponse(201, AppMessages.INSERTED));
        }
        #endregion

        #region PUT
        [Authorize(Roles = "Manager,HR")]
        [HttpPut]
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

            return Ok(new ApiResponse(201, AppMessages.UPDATED));
        }

        #endregion

        #region DELETE
        [Authorize(Roles = "Manager,HR")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAttendenceAsync(int id)
        {
            var attendenceSpec = new AttendanceWithEmployeeSpec(id);
            var attendence = _uof.GetRepository<Attendence>().FindSpec(attendenceSpec).Result;
            _uof.GetRepository<Attendence>().DeleteAsync(attendence);
            await _uof.Commit();
            return Ok(new ApiResponse(201, AppMessages.DELETED));
        }
        #endregion

    }
}
