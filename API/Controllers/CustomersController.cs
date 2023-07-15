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
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly ISearchCustomerService _searchCustomerService;

        public CustomersController(IGenericRepository<Customer> customerRepo,
                                  IMapper mapper,
                                  IPhoneNumberValidator phoneNumberValidator,
                                  ISearchCustomerService searchCustomerService
                                   )
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchCustomerService = searchCustomerService;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> GetAllCustomerAsync()
        {
            var customers = await _customerRepo.GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<ReadCustomerDto>>(customers));
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<CreateCustomerDto, Customer>(createCustomerDto);
                _customerRepo.InsertAsync(customer);
                await _customerRepo.SaveChangesAsync();

                return Ok(_mapper.Map<Customer, CreateCustomerDto>(customer));
            }
            else
            {
                return BadRequest(new { Detail = $"Invalid Phone Number : {createCustomerDto.CustomerPhoneNumber}" });
            }
        }


        [HttpPut("Update")]
        public async Task<ActionResult> UpdateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<CreateCustomerDto, Customer>(createCustomerDto);
            _customerRepo.UpdateAsync(customer);
            await _customerRepo.SaveChangesAsync();

            return Ok(_mapper.Map<Customer, CreateCustomerDto>(customer));
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> SearchWithCriteria(string? Name = null, string? PhoneNumber = null)
        {
            var result = await _searchCustomerService.SearchWithCriteria(Name, PhoneNumber);
            return Ok(result);
        }
    }
}
