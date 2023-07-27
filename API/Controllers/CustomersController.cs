using Application.DTOs.Attendance;
using Application.DTOs.Customer;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BannedCustomerSpec;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.CustomerSpec;
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
        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<Pagination<ReadCustomerDto>>> GetAllCustomerAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new CustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalCustomers = await _uof.GetRepository<Customer>().CountAsync(spec);

            var customers = await _uof.GetRepository<Customer>().FindAllSpec(spec);

            var mappedcustomers = _mapper.Map<IReadOnlyList<ReadCustomerDto>>(customers);

            var paginationData = new Pagination<ReadCustomerDto>(spec.PageIndex, spec.PageSize, totalCustomers, mappedcustomers);

            return Ok(paginationData);
        }

        [HttpGet("GetCustomerById")]
        public async Task<ActionResult> GetCustomerByIdAsync(int id)
        {
            if (await _uof.GetRepository<Customer>().Exists(id))
            {
                var author = await _uof.GetRepository<Customer>().GetByIdAsync(id);
                return Ok(_mapper.Map<Customer, ReadCustomerDto>(author));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("SearchCustomers")]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> SearchWithCriteria(string? Name = null, string? PhoneNumber = null)
        {
            var result = await _searchCustomerService.SearchWithCriteria(Name, PhoneNumber);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertCustomer")]
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
        [HttpPut("UpdateCustomer")]
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
        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomerAsync(int id)
        {
            var bannedCustomerSpec = new BannedCustomerWithEmployeeAndCustomerSpec(null, id);
            var result = _uof.GetRepository<BannedCustomer>().FindAllSpec(bannedCustomerSpec).Result;
            if (result.Count() > 0)
                return BadRequest(AppMessages.FAILED_DELETE);
            else
            {
                var customerSpec = new CustomerSpec(id);
                var customer = _uof.GetRepository<Customer>().FindSpec(customerSpec).Result;
                _uof.GetRepository<Customer>().DeleteAsync(customer);
                await _uof.Commit();
                return Ok(AppMessages.DELETED);
            }
        }
        #endregion

    }
}
