using Application.DTOs.Attendance;
using Application.DTOs.BannedCustomer;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Specifications.AttendanceSpec;
using Application;
using Infrastructure.Specifications.BannedCustomerSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Application.Handlers;
using Application.Interfaces.IValidators;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannedCustomersController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IBannedCustomerServices _searchForBannedCustomerService;
        private readonly ILogger<BannedCustomersController> _logger;

        public BannedCustomersController(IUnitOfWork uof,
                                  IMapper mapper,
                                  IBannedCustomerServices searchForBannedCustomerService,
                                  ILogger<BannedCustomersController> logger
                                   )
        {
            _uof = uof;
            _mapper = mapper;
            _searchForBannedCustomerService = searchForBannedCustomerService;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllBannedCustomers")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> GetAllBannedCustomerAsync()
        {
            var customers = await _uof.GetRepository<BannedCustomer>().GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<BannedCustomer>, IReadOnlyList<ReadBannedCustomerDto>>(customers));
        }

        [HttpGet("GetBannedCustomersWithDetails")]
        public async Task<ActionResult<Pagination<ReadBannedCustomerDto>>> GetBannedCustomersWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400));

            var spec = new BannedCustomerWithEmployeeAndCustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalBannedCustomers = await _uof.GetRepository<BannedCustomer>().CountAsync(spec);
            if (totalBannedCustomers == 0)
                return NotFound(new ApiResponse(404));

            var bannedCustomers = await _uof.GetRepository<BannedCustomer>().FindAllSpec(spec);

            var mappedbannedCustomers = _mapper.Map<IReadOnlyList<ReadBannedCustomerDto>>(bannedCustomers);

            var paginationData = new Pagination<ReadBannedCustomerDto>(spec.PageIndex, spec.PageSize, totalBannedCustomers, mappedbannedCustomers);

            return Ok(paginationData);
        }

        [HttpGet("GetBannedCustomerById")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<BannedCustomer>().Exists(id);

            if (exists)
            {
                var bannedCustomers = await _uof.GetRepository<BannedCustomer>().GetByIdAsync(id);

                if (bannedCustomers == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [HttpGet("GetBannedCustomerByIdWithIncludesAsync")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<BannedCustomer>().Exists(id);

            if (exists)
            {
                var spec = new BannedCustomerWithEmployeeAndCustomerSpec(id);
                var bannedCustomers = await _uof.GetRepository<BannedCustomer>().FindSpec(spec);

                if (bannedCustomers == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }


        [HttpGet("SearchBannedCustomersByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> SearchByCriteria(string? EmpName = null, string? CustomerName = null)
        {
            var result = await _searchForBannedCustomerService.SearchForBannedCustomer(EmpName, CustomerName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertBannedCustomer")]
        public async Task<ActionResult> InsertannedCustomerAsync(CreateBannedCustomerDto createBannedCustomerDto)
        {
            var result = await _uof.GetRepository<Customer>().Exists(createBannedCustomerDto.CustomerId);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            var bannedCustomer = _mapper.Map<CreateBannedCustomerDto, BannedCustomer>(createBannedCustomerDto);
            _uof.GetRepository<BannedCustomer>().InsertAsync(bannedCustomer);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteBannedCustomer")]
        public async Task<ActionResult> DeleteBannedCustomerAsync(int id)
        {
            var bannedCustomerSpec = new BannedCustomerWithEmployeeAndCustomerSpec(id);
            var bannedCustomer = _uof.GetRepository<BannedCustomer>().FindSpec(bannedCustomerSpec).Result;
            if (bannedCustomer == null)
                return NotFound(new ApiResponse(404));
            _uof.GetRepository<BannedCustomer>().DeleteAsync(bannedCustomer);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
