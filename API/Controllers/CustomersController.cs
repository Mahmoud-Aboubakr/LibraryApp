using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
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
        private readonly ICustomerServices _searchCustomerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IUnitOfWork uof,
                                  IMapper mapper,
                                  IPhoneNumberValidator phoneNumberValidator,
                                  ICustomerServices searchCustomerService,
                                  ILogger<CustomersController> logger
                                   )
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchCustomerService = searchCustomerService;
            _logger = logger;
        }

        #region GET
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

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("Search")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> SearchWithCriteria(string? Name = null, string? PhoneNumber = null)
        {
            var result = await _searchCustomerService.SearchWithCriteria(Name, PhoneNumber);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<CreateCustomerDto, Customer>(createCustomerDto);
                _uof.GetRepository<Customer>().InsertAsync(customer);
                await _uof.Commit();

                return StatusCode(201, AppMessages.INSERTED);
            }
            else
            {
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {createCustomerDto.CustomerPhoneNumber}" });
            }
        }
        #endregion

        #region PUT
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateCustomerAsync(ReadCustomerDto readCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(readCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<ReadCustomerDto, Customer>(readCustomerDto);
                _uof.GetRepository<Customer>().UpdateAsync(customer);
                await _uof.Commit();

                return Ok(AppMessages.UPDATED);
            }
            else
            {
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {readCustomerDto.CustomerPhoneNumber}" });
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> DeleteBannedCustomerAsync(ReadCustomerDto readCustomerDto)
        {
            var Customer = _mapper.Map<ReadCustomerDto, Customer>(readCustomerDto);
            _uof.GetRepository<Customer>().DeleteAsync(Customer);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
