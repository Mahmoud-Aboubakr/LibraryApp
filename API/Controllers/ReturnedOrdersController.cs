using Application.DTOs.BookOrderDetails;
using Application.DTOs.Order;
using Application.DTOs.Publisher;
using Application.DTOs.ReturnedOrder;
using Application.DTOs.ReturnOrderDetails;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.AppServices;
using Infrastructure.Specifications.BookOrderDetailsSpec;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.ReturnOrderSpec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnedOrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IReturnedOrderServices _returnedOrderServices;
        private readonly ILogger<ReturnedOrdersController> _logger;

        public ReturnedOrdersController(IUnitOfWork uof,
            IMapper mapper,
            IReturnedOrderServices returnedOrderServices,
            ILogger<ReturnedOrdersController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _returnedOrderServices = returnedOrderServices;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllReturnedOrders")]
        public async Task<ActionResult<IReadOnlyList<ReadReturnedOrderDto>>> GetAllReturnedOrders()
        {
            var returnedOrders = await _uof.GetRepository<ReturnedOrder>().GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<ReturnedOrder>, IReadOnlyList<ReadReturnedOrderDto>>(returnedOrders));
        }

        [HttpGet("GetAllReturnedOrdersWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadReturnedOrderDto>>> GetAllReturnedOrdersWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new ReturnedOrderWithCustomerSpec(pagesize, pageindex, isPagingEnabled);
            var totalReturns = await _uof.GetRepository<ReturnedOrder>().CountAsync(spec);
            var Returns = await _uof.GetRepository<ReturnedOrder>().FindAllSpec(spec);
            var mappedReturns = _mapper.Map<IReadOnlyList<ReadReturnedOrderDto>>(Returns);
            var paginationData = new Pagination<ReadReturnedOrderDto>(spec.PageIndex, spec.PageSize, totalReturns, mappedReturns);
            return Ok(paginationData);
        }

        [HttpGet("GetAllReturnedOrderDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadReturnOrderDetailsDto>>> GetAllReturnedOrderDetailsAsync()
        {
            var returnOrdersDetails = await _uof.GetRepository<ReturnOrderDetails>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<ReturnOrderDetails>, IReadOnlyList<ReadReturnOrderDetailsDto>>(returnOrdersDetails));
        }


        [HttpGet("GetAllReturnedOrderDetailsWithIncludes")]
        public async Task<ActionResult<IEnumerable<ReadReturnOrderDetailsDto>>> GetAllReturnedOrderDetailsWithIncludesAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new ReturnOrderDetailsWithBookAndCustomerSpec(pagesize, pageindex, isPagingEnabled);
            var totalReturns = await _uof.GetRepository<ReturnOrderDetails>().CountAsync(spec);
            var Returns = await _uof.GetRepository<ReturnOrderDetails>().FindAllSpec(spec);
            var mappedReturns = _mapper.Map<IReadOnlyList<ReadReturnOrderDetailsDto>>(Returns);
            var paginationData = new Pagination<ReadReturnOrderDetailsDto>(spec.PageIndex, spec.PageSize, totalReturns, mappedReturns);
            return Ok(paginationData);
        }


        [HttpGet("GetReturnedOrderById")]
        public async Task<ActionResult<ReadReturnedOrderDto>> GetReturnedOrderById(int id)
        {
            var exists = await _uof.GetRepository<ReturnedOrder>().Exists(id);

            if (exists)
            {
                var returnedorder = await _uof.GetRepository<ReturnedOrder>().GetByIdAsync(id);

                if (returnedorder == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnedOrderDto>(returnedorder));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("GetReturnedOrderByIdWithIncludes")]
        public async Task<ActionResult<ReadReturnedOrderDto>> GetReturnedOrderByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<ReturnedOrder>().Exists(id);

            if (exists)
            {
                var spec = new ReturnedOrderWithCustomerSpec(id);
                var returnedorder = await _uof.GetRepository<ReturnedOrder>().FindSpec(spec);

                if (returnedorder == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnedOrderDto>(returnedorder));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchOrderWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReturnedOrder>>> SearchReturnedOrderByCriteria(int? originorderId = null, int? customerId = null, string? customerName = null, decimal? totalPrice = null, DateTime? returndate = null)
        {
            var result = await _returnedOrderServices.SearchReturnedOrders(originorderId, customerId, customerName, totalPrice, returndate);
            return Ok(result);
        }

        [HttpGet("GetReturnedOrderDetailsById")]
        public async Task<ActionResult<ReadReturnOrderDetailsDto>> GetReturnedOrderDetailsById(int id)
        {
            var exists = await _uof.GetRepository<ReturnOrderDetails>().Exists(id);

            if (exists)
            {
                var returnOrdersDetails = await _uof.GetRepository<ReturnOrderDetails>().GetByIdAsync(id);

                if (returnOrdersDetails == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnOrderDetailsDto>(returnOrdersDetails));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetReturnedOrderDetailsByIdWithIncludes")]
        public async Task<ActionResult<ReadReturnOrderDetailsDto>> GetReturnedOrderDetailsByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<ReturnOrderDetails>().Exists(id);

            if (exists)
            {
                var spec = new ReturnOrderDetailsWithBookAndCustomerSpec(id);
                var returnOrdersDetails = await _uof.GetRepository<ReturnOrderDetails>().FindSpec(spec);

                if (returnOrdersDetails == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnOrderDetailsDto>(returnOrdersDetails));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchReturnOrderDetailsWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReturnOrderDetails>>> SearchReturnOrderDetailsByCriteria(int? returnedorderId = null, int? bookId = null, string? customerName = null, string? bookTitle = null)
        {
            var result = await _returnedOrderServices.SearchReturnedOrdersDetails(returnedorderId, bookId, customerName, bookTitle);
            return Ok(result);
        }

        #endregion

        #region POST
        [HttpPost("InsertReturnedOrder")]
        public async Task<ActionResult> InsertReturnedOrderAsync(CreateReturnedOrderDto createReturnedOrder)
        {
            var IsValidOriginOrderId = await _uof.GetRepository<Order>().Exists(createReturnedOrder.OriginOrderId);
            if (!IsValidOriginOrderId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnedOrder.OriginOrderId}" });
            var IsValidCustomerId = await _uof.GetRepository<Customer>().Exists(createReturnedOrder.CustomerId);
            if (!IsValidCustomerId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnedOrder.CustomerId}" });

            var order = await _uof.GetRepository<Order>().GetByIdAsync(createReturnedOrder.OriginOrderId);
            var orderDate = order.OrderDate;
            var returnDate = DateTime.Now;
            if (!_returnedOrderServices.IsInReturnInterval(returnDate, orderDate))
                return BadRequest(new { Detail = AppMessages.FAILED_RETURN});

            createReturnedOrder.TotalPrice = order.TotalPrice;
            var returnedOrder = _mapper.Map<CreateReturnedOrderDto, ReturnedOrder>(createReturnedOrder);
            _uof.GetRepository<ReturnedOrder>().InsertAsync(returnedOrder);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }


        [HttpPost("InsertReturnOrderDetails")]
        public async Task<ActionResult> InsertReturnOrderDetailsAsync(CreateReturnOrderDetailsDto createReturnOrderDetails)
        {
            var IsValidOriginReturnedOrderId = await _uof.GetRepository<ReturnedOrder>().Exists(createReturnOrderDetails.ReturnedOrderId);
            if (!IsValidOriginReturnedOrderId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.ReturnedOrderId}" });
            var IsValidBookId = await _uof.GetRepository<Book>().Exists(createReturnOrderDetails.BookId);
            if (!IsValidBookId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.BookId}" });

            var returnedorder = await _uof.GetRepository<ReturnedOrder>().GetByIdAsync(createReturnOrderDetails.ReturnedOrderId);

            var bookOrderDetailsSpec = new BookOrderDetailsWithBookAndCustomerSpec(null, null, returnedorder.OriginOrderId);
            var originalorderDetails = _uof.GetRepository<BookOrderDetails>().FindAllSpec(bookOrderDetailsSpec).Result;
            
            var found = false;
            foreach (var item in originalorderDetails)
            {
                if (createReturnOrderDetails.BookId == item.BookId)
                {
                    found = true;
                    if (!_returnedOrderServices.CheckQuantity(createReturnOrderDetails.Quantity, item.Quantity))
                        return BadRequest(new { Detail = AppMessages.INVALID_QUANTITY});
                    break;
                }
            }
            if (!found)
            {
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.BookId}" });
            }

            var returnedbook = await _uof.GetRepository<Book>().GetByIdAsync(createReturnOrderDetails.BookId);
            createReturnOrderDetails.Price = returnedbook.Price;

            var returnedOrderDetails = _mapper.Map<CreateReturnOrderDetailsDto, ReturnOrderDetails>(createReturnOrderDetails);
            _uof.GetRepository<ReturnOrderDetails>().InsertAsync(returnedOrderDetails);
            await _uof.Commit();
            _returnedOrderServices.IncreaseQuantity(createReturnOrderDetails.BookId, createReturnOrderDetails.Quantity);
            return StatusCode(201, AppMessages.RETURNED);
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteReturnedOrderAsync")]
        public async Task<ActionResult> DeleteReturnedOrderAsync(int returnedorderId)
        {
            var result = await _uof.GetRepository<ReturnedOrder>().Exists(returnedorderId);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {returnedorderId}" });
            _returnedOrderServices.DeleteReturnedOrderAsync(returnedorderId);
            return Ok(AppMessages.DELETED);
        }

        [HttpDelete("DeleteReturnedOrderDetailsAsync")]
        public async Task<IActionResult> DeleteReturnedOrderDetailsAsync(ReadReturnOrderDetailsDto readReturnOrderDetailsDto)
        {
            var returnOrderDetails = _mapper.Map<ReadReturnOrderDetailsDto, ReturnOrderDetails>(readReturnOrderDetailsDto);
            _uof.GetRepository<ReturnOrderDetails>().DeleteAsync(returnOrderDetails);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
    }
}
