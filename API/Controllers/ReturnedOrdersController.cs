using Application.DTOs.BookOrderDetails;
using Application.DTOs.Order;
using Application.DTOs.ReturnedOrder;
using Application.DTOs.ReturnOrderDetails;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.AppServices;
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
        private readonly IUnitOfWork<ReturnedOrder> _uof;
        private readonly IUnitOfWork<ReturnOrderDetails> _returnOrderDetailsUof;
        private readonly IUnitOfWork<Order> _orderUof;
        private readonly IUnitOfWork<Customer> _customerUof;
        private readonly IUnitOfWork<Book> _bookUof;
        private readonly IUnitOfWork<BookOrderDetails> _bookOrderDetailsUof;
        private readonly IMapper _mapper;
        private readonly IReturnedOrderServices _returnedOrderServices;
        private readonly ILogger<ReturnedOrdersController> _logger;

        public ReturnedOrdersController(IUnitOfWork<ReturnedOrder> uof,
            IUnitOfWork<ReturnOrderDetails> returnOrderDetailsUof,
            IUnitOfWork<Order> orderUof,
            IUnitOfWork<Customer> customerUof,
            IUnitOfWork<Book> bookUof,
            IUnitOfWork<BookOrderDetails> bookOrderDetailsUof,
            IMapper mapper,
            IReturnedOrderServices returnedOrderServices,
            ILogger<ReturnedOrdersController> logger)
        {
            _uof = uof;
            _returnOrderDetailsUof = returnOrderDetailsUof;
            _orderUof = orderUof;
            _customerUof = customerUof;
            _bookUof = bookUof;
            _orderUof = orderUof;
            _mapper = mapper;
            _returnedOrderServices = returnedOrderServices;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllReturnedOrders")]
        public async Task<ActionResult<IReadOnlyList<ReadReturnedOrderDto>>> GetAllReturnedOrders()
        {
            var returnedOrders = await _uof.GetRepository().GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<ReturnedOrder>, IReadOnlyList<ReadReturnedOrderDto>>(returnedOrders));
        }

        [HttpGet("GetAllReturnedOrdersWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadReturnedOrderDto>>> GetAllReturnedOrdersWithDetails()
        {
            var spec = new ReturnedOrderWithCustomerSpec();
            var returnedOrders = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<ReturnedOrder>, IEnumerable<ReadReturnedOrderDto>>(returnedOrders));
        }

        [HttpGet("GetAllReturnedOrderDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadReturnOrderDetailsDto>>> GetAllReturnedOrderDetailsAsync()
        {
            var returnOrdersDetails = await _returnOrderDetailsUof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<ReturnOrderDetails>, IReadOnlyList<ReadReturnOrderDetailsDto>>(returnOrdersDetails));
        }


        [HttpGet("GetAllReturnedOrderDetailsWithIncludes")]
        public async Task<ActionResult<IEnumerable<ReadReturnOrderDetailsDto>>> GetAllReturnedOrderDetailsWithIncludesAsync()
        {
            var spec = new ReturnOrderDetailsWithBookAndCustomerSpec();
            var returnOrdersDetails = await _returnOrderDetailsUof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<ReturnOrderDetails>, IEnumerable<ReadReturnOrderDetailsDto>>(returnOrdersDetails));
        }


        [HttpGet("GetReturnedOrderById")]
        public async Task<ActionResult<ReadReturnedOrderDto>> GetReturnedOrderById(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var returnedorder = await _uof.GetRepository().GetByIdAsync(id);

                if (returnedorder == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnedOrderDto>(returnedorder));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("GetReturnedOrderByIdWithIncludes")]
        public async Task<ActionResult<ReadReturnedOrderDto>> GetReturnedOrderByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var spec = new ReturnedOrderWithCustomerSpec(id);
                var returnedorder = await _uof.GetRepository().FindSpec(spec);

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
            var exists = await _returnOrderDetailsUof.GetRepository().Exists(id);

            if (exists)
            {
                var returnOrdersDetails = await _returnOrderDetailsUof.GetRepository().GetByIdAsync(id);

                if (returnOrdersDetails == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadReturnOrderDetailsDto>(returnOrdersDetails));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetReturnedOrderDetailsByIdWithIncludes")]
        public async Task<ActionResult<ReadReturnOrderDetailsDto>> GetReturnedOrderDetailsByIdWithIncludesAsync(int id)
        {
            var exists = await _returnOrderDetailsUof.GetRepository().Exists(id);

            if (exists)
            {
                var spec = new ReturnOrderDetailsWithBookAndCustomerSpec(id);
                var returnOrdersDetails = await _returnOrderDetailsUof.GetRepository().FindSpec(spec);

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
            var IsValidOriginOrderId = await _orderUof.GetRepository().Exists(createReturnedOrder.OriginOrderId);
            if (!IsValidOriginOrderId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnedOrder.OriginOrderId}" });
            var IsValidCustomerId = await _customerUof.GetRepository().Exists(createReturnedOrder.CustomerId);
            if (!IsValidCustomerId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnedOrder.CustomerId}" });

            var order = await _orderUof.GetRepository().GetByIdAsync(createReturnedOrder.OriginOrderId);
            var orderDate = order.OrderDate;
            var returnDate = DateTime.Now;
            if (!_returnedOrderServices.IsInReturnInterval(returnDate, orderDate))
                return BadRequest(new { Detail = $"You Can't Return This Order" });

            createReturnedOrder.TotalPrice = order.TotalPrice;
            var returnedOrder = _mapper.Map<CreateReturnedOrderDto, ReturnedOrder>(createReturnedOrder);
            _uof.GetRepository().InsertAsync(returnedOrder);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }


        [HttpPost("InsertReturnOrderDetails")]
        public async Task<ActionResult> InsertReturnOrderDetailsAsync(CreateReturnOrderDetailsDto createReturnOrderDetails)
        {
            var IsValidOriginReturnedOrderId = await _uof.GetRepository().Exists(createReturnOrderDetails.ReturnedOrderId);
            if (!IsValidOriginReturnedOrderId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.ReturnedOrderId}" });
            var IsValidBookId = await _bookUof.GetRepository().Exists(createReturnOrderDetails.BookId);
            if (!IsValidBookId)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.BookId}" });

            var returnedorder = await _uof.GetRepository().GetByIdAsync(createReturnOrderDetails.ReturnedOrderId);
            var originalorderDetails = await _bookOrderDetailsUof.GetRepository().GetAllWithWhere(x => x.OrderId == returnedorder.OriginOrderId);

            var found = false;
            foreach (var item in originalorderDetails)
            {
                if (createReturnOrderDetails.BookId == item.BookId)
                {
                    found = true;
                    if (!_returnedOrderServices.CheckQuantity(createReturnOrderDetails.Quantity, item.Quantity))
                        return BadRequest(new { Detail = $"Enter Valid Returned Quantity" });
                    break;
                }
            }
            if (!found)
            {
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {createReturnOrderDetails.BookId}" });
            }

            var returnedbook = await _bookUof.GetRepository().GetByIdAsync(createReturnOrderDetails.BookId);
            createReturnOrderDetails.Price = returnedbook.Price;

            var returnedOrderDetails = _mapper.Map<CreateReturnOrderDetailsDto, ReturnOrderDetails>(createReturnOrderDetails);
            _returnOrderDetailsUof.GetRepository().InsertAsync(returnedOrderDetails);
            await _returnOrderDetailsUof.Commit();
            _returnedOrderServices.IncreaseQuantity(createReturnOrderDetails.BookId, createReturnOrderDetails.Quantity);
            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteReturnedOrderAsync")]
        public async Task<ActionResult> DeleteReturnedOrderAsync(int returnedorderId)
        {
            var result = await _uof.GetRepository().Exists(returnedorderId);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {returnedorderId}" });
            _returnedOrderServices.DeleteReturnedOrderAsync(returnedorderId);
            return Ok(AppMessages.DELETED);
        }

        [HttpDelete("DeleteReturnedOrderDetailsAsync")]
        public async Task<IActionResult> DeleteReturnedOrderDetailsAsync(ReadReturnOrderDetailsDto readReturnOrderDetailsDto)
        {
            var returnOrderDetails = _mapper.Map<ReadReturnOrderDetailsDto, ReturnOrderDetails>(readReturnOrderDetailsDto);
            _returnOrderDetailsUof.GetRepository().DeleteAsync(returnOrderDetails);
            await _returnOrderDetailsUof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}
