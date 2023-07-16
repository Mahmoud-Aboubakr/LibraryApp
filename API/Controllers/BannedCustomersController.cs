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
        /*
        private readonly IGenericRepository<BannedCustomer> _bannedCustomerRepo;
        private readonly IMapper _mapper;
        private readonly ISearchBannedCustomerService _searchForBannedCustomerService;

        public BannedCustomersController(IGenericRepository<BannedCustomer> bannedCustomerRepo,
                                  IMapper mapper,
                                  ISearchBannedCustomerService searchForBannedCustomerService
                                   )
        {
            _bannedCustomerRepo = bannedCustomerRepo;
            _mapper = mapper;
            _searchForBannedCustomerService = searchForBannedCustomerService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> GetAllBannedCustomerAsync()
        {
            var customers = await _bannedCustomerRepo.GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<BannedCustomer>, IReadOnlyList<ReadBannedCustomerDto>>(customers));
        }

        [HttpGet("GetBannedCustomersWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> GetBannedCustomersWithDetails()
        {
            var bannedCustomers = await _bannedCustomerRepo.GetAllListWithIncludesAsync(new Expression<Func<BannedCustomer, object>>[] { x => x.Employee, x => x.Customer });
            return Ok(_mapper.Map<IReadOnlyList<BannedCustomer>, IReadOnlyList<ReadBannedCustomerDto>>(bannedCustomers));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetById(int id)
        {
            var exists = await _bannedCustomerRepo.Exists(id);

            if (exists)
            {
                var bannedCustomers = await _bannedCustomerRepo.GetByIdAsync(id);

                if (bannedCustomers == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpGet("GetByIdWithIncludesAsync")]
        public async Task<ActionResult<ReadBannedCustomerDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _bannedCustomerRepo.Exists(id);

            if (exists)
            {
                var bannedCustomers = await _bannedCustomerRepo.GetByIdAsyncWithIncludes(id, new Expression<Func<BannedCustomer, object>>[] { x => x.Employee, x => x.Customer });

                if (bannedCustomers == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBannedCustomerDto>(bannedCustomers));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertannedCustomerAsync(CreateBannedCustomerDto createBannedCustomerDto)
        {
            var bannedCustomer = _mapper.Map<CreateBannedCustomerDto, BannedCustomer>(createBannedCustomerDto);
            _bannedCustomerRepo.InsertAsync(bannedCustomer);
            await _bannedCustomerRepo.SaveChangesAsync();

            return Ok(_mapper.Map<BannedCustomer, CreateBannedCustomerDto>(bannedCustomer));
        }

        [HttpDelete]
        public async Task DeleteBannedCustomerAsync(ReadBannedCustomerDto readBannedCustomerDto)
        {
            var bannedCustomer = _mapper.Map<ReadBannedCustomerDto, BannedCustomer>(readBannedCustomerDto);
            _bannedCustomerRepo.DeleteAsync(bannedCustomer);
            await _bannedCustomerRepo.SaveChangesAsync();
        }


        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBannedCustomerDto>>> SearchByCriteria(string? EmpName = null, string? CustomerName = null)
        {
            var result = await _searchForBannedCustomerService.SearchForBannedCustomer(EmpName , CustomerName);
            return Ok(result);
        }
        */
    }
}
