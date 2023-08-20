using Application.DTOs.Attendance;
using Application.DTOs.Customer;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.AppServices;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BannedCustomerSpec;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.CustomerSpec;
using Infrastructure.Specifications.EmployeeSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadCustomerDto>>> GetAllCustomerAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));

            var spec = new CustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalCustomers = await _uof.GetRepository<Customer>().CountAsync(spec);
            
            var customers = await _uof.GetRepository<Customer>().FindAllSpec(spec);

            var mappedcustomers = _mapper.Map<IReadOnlyList<ReadCustomerDto>>(customers);

            if (mappedcustomers == null || totalCustomers == 0)
            {
                return NotFound(new ApiResponse(404 , AppMessages.NULL_DATA));
            }

            var paginationData = new Pagination<ReadCustomerDto>(spec.Skip, spec.Take, totalCustomers, mappedcustomers);

            return Ok(paginationData);
        }

        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerByIdAsync(string id)
        {
            if (await _uof.GetRepository<Customer>().Exists(int.Parse(id)))
            {
                var author = await _uof.GetRepository<Customer>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Customer, ReadCustomerDto>(author));
            }

            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadCustomerDto>>> SearchCustomerWithCriteria(string? Name = null, string? PhoneNumber = null)
        {
            if (!_phoneNumberValidator.IsValidPhoneNumber(PhoneNumber))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var result = await _searchCustomerService.SearchWithCriteria(Name, PhoneNumber);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404 , AppMessages.NOTFOUND_SEARCHDATA));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [Authorize(Roles = "Manager,Librarian")]
        [HttpPost]
        public async Task<ActionResult> InsertCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createCustomerDto.CustomerPhoneNumber))
            {
                var customer = _mapper.Map<CreateCustomerDto, Customer>(createCustomerDto);
                _uof.GetRepository<Customer>().InsertAsync(customer);
                await _uof.Commit();

                return Ok(new ApiResponse(201, AppMessages.INSERTED));
            }
            else
            {
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            }
        }
        #endregion

        #region PUT
        [Authorize(Roles = "Manager,Librarian")]
        [HttpPut]
        public async Task<ActionResult> UpdateCustomerAsync(ReadCustomerDto readCustomerDto)
        {
            var result = await _uof.GetRepository<Customer>().Exists(readCustomerDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_phoneNumberValidator.IsValidPhoneNumber(readCustomerDto.CustomerPhoneNumber))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var customer = _mapper.Map<ReadCustomerDto, Customer>(readCustomerDto);
            _uof.GetRepository<Customer>().UpdateAsync(customer);
            await _uof.Commit();

            return Ok(new ApiResponse(201, AppMessages.UPDATED));
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "Manager,Librarian")]
        [HttpDelete]
        public async Task<ActionResult> DeleteCustomerAsync(string id)
        {
            var bannedCustomerSpec = new BannedCustomerWithEmployeeAndCustomerSpec(null, int.Parse(id));
            var result = _uof.GetRepository<BannedCustomer>().FindAllSpec(bannedCustomerSpec).Result;
            if (result.Count() > 0)
                return BadRequest(new ApiResponse(400, AppMessages.FAILED_DELETE));
            else
            {
                var customerSpec = new CustomerSpec(int.Parse(id));
                var customer = _uof.GetRepository<Customer>().FindSpec(customerSpec).Result;
                _uof.GetRepository<Customer>().DeleteAsync(customer);
                await _uof.Commit();
                return Ok(new ApiResponse(201, AppMessages.DELETED));
            }
        }
        #endregion

    }
}
