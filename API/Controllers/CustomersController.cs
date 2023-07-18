using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly ISearchCustomerService _searchCustomerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IUnitOfWork uof,
                                  IMapper mapper,
                                  IPhoneNumberValidator phoneNumberValidator,
                                  ISearchCustomerService searchCustomerService,
                                  ILogger<CustomersController> logger
                                   )
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchCustomerService = searchCustomerService;
            _logger = logger;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> GetAllCustomerAsync()
        {
            var customers = await _uof.GetRepository<Customer>().GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<ReadCustomerDto>>(customers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerByIdAsync(int id)
        {
            if (await _uof.GetRepository<Customer>().Exists(id))
            {
                var author = await _uof.GetRepository<Customer>().GetByIdAsync(id);
                return Ok(_mapper.Map<Customer, ReadCustomerDto>(author));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<CreateCustomerDto, Customer>(createCustomerDto);
                _uof.GetRepository<Customer>().InsertAsync(customer);
                await _uof.Commit();

                return Ok(_mapper.Map<Customer, CreateCustomerDto>(customer));
            }
            else
            {
                return BadRequest(new { Detail = $"Invalid Phone Number : {createCustomerDto.CustomerPhoneNumber}" });
            }
        }


        [HttpPut("Update")]
        public async Task<ActionResult> UpdateCustomerAsync(ReadCustomerDto readCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(readCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<ReadCustomerDto, Customer>(readCustomerDto);
                _uof.GetRepository<Customer>().UpdateAsync(customer);
                await _uof.Commit();

                return Ok(_mapper.Map<Customer, ReadCustomerDto>(customer));
            }
            else
            {
                return BadRequest(new { Detail = $"Invalid Phone Number : {readCustomerDto.CustomerPhoneNumber}" });
            }
        }

        [HttpDelete]
        public async Task DeleteBannedCustomerAsync(ReadCustomerDto readCustomerDto)
        {
            var Customer = _mapper.Map<ReadCustomerDto, Customer>(readCustomerDto);
            _uof.GetRepository<Customer>().DeleteAsync(Customer);
            await _uof.Commit();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> SearchWithCriteria(string? Name = null, string? PhoneNumber = null)
        {
            var result = await _searchCustomerService.SearchWithCriteria(Name, PhoneNumber);
            return Ok(result);
        }
        
    }
}
