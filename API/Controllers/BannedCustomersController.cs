using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannedCustomersController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ISearchBannedCustomerService _searchForBannedCustomerService;
        private readonly ILogger<BannedCustomersController> _logger;

        public BannedCustomersController(IUnitOfWork uof,
                                  IMapper mapper,
                                  ISearchBannedCustomerService searchForBannedCustomerService,
                                  ILogger<BannedCustomersController> logger
                                   )
        {
            _uof = uof;
            _mapper = mapper;
            _searchForBannedCustomerService = searchForBannedCustomerService;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> GetAllBannedCustomerAsync()
        {
            var customers = await _uof.GetRepository<BannedCustomer>().GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<BannedCustomer>, IReadOnlyList<ReadBannedCustomerDto>>(customers));
        }

        [HttpGet("GetBannedCustomersWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> GetBannedCustomersWithDetails()
        {
            var bannedCustomers = await _uof.GetRepository<BannedCustomer>().GetAllListWithIncludesAsync(new Expression<Func<BannedCustomer, object>>[] { x => x.Employee, x => x.Customer });
            return Ok(_mapper.Map<IReadOnlyList<BannedCustomer>, IReadOnlyList<ReadBannedCustomerDto>>(bannedCustomers));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<BannedCustomer>().Exists(id);

            if (exists)
            {
                var bannedCustomers = await _uof.GetRepository<BannedCustomer>().GetByIdAsync(id);

                if (bannedCustomers == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return NotFound(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpGet("GetByIdWithIncludesAsync")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<BannedCustomer>().Exists(id);

            if (exists)
            {
                var bannedCustomers = await _uof.GetRepository<BannedCustomer>().GetByIdAsyncWithIncludes(id, new Expression<Func<BannedCustomer, object>>[] { x => x.Employee, x => x.Customer });

                if (bannedCustomers == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return NotFound(new { Detail = $"Id : {id} is not vaild !!" });
        }


        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> SearchByCriteria(string? EmpName = null, string? CustomerName = null)
        {
            var result = await _searchForBannedCustomerService.SearchForBannedCustomer(EmpName, CustomerName);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertannedCustomerAsync(CreateBannedCustomerDto createBannedCustomerDto)
        {
            var bannedCustomer = _mapper.Map<CreateBannedCustomerDto, BannedCustomer>(createBannedCustomerDto);
            _uof.GetRepository<BannedCustomer>().InsertAsync(bannedCustomer);
            await _uof.Commit();

            return StatusCode(201, "Customer Inserted Successfully");
        }
        #endregion

        #region PUT

        #endregion

        #region DELETE
        [HttpDelete]
        public async Task DeleteBannedCustomerAsync(ReadBannedCustomerDto readBannedCustomerDto)
        {
            var bannedCustomer = _mapper.Map<ReadBannedCustomerDto, BannedCustomer>(readBannedCustomerDto);
            _uof.GetRepository<BannedCustomer>().DeleteAsync(bannedCustomer);
            await _uof.Commit();
        }
        #endregion

    }
}
