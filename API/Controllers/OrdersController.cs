using Application.DTOs.BookOrderDetails;
using Application.DTOs.Employee;
using Application.DTOs.Order;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.AppServices;
using Infrastructure.Specifications.OrderSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IUnitOfWork uof,
            IMapper mapper,
            IOrderServices orderServices,
            INumbersValidator numbersValidator,
            ILogger<OrdersController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _orderServices = orderServices;
            _numbersValidator = numbersValidator;
            _logger = logger;
        }

        #region Get
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<ReadOrderDto>>> GetAllOrders(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));
            }
            var spec = new OrderWithCustomerSpec(pagesize, pageindex, isPagingEnabled);
            var totalOrders = await _uof.GetRepository<Order>().CountAsync(spec);
            var orders = await _uof.GetRepository<Order>().FindAllSpec(spec);
            var mappedOrders = _mapper.Map<IReadOnlyList<ReadOrderDto>>(orders);
            if (mappedOrders == null || totalOrders == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            var paginationData = new Pagination<ReadOrderDto>(spec.PageIndex, spec.PageSize, totalOrders, mappedOrders);
            return Ok(paginationData);
        }

        [HttpGet("GetByIdWithIncludes")]
        public async Task<ActionResult<ReadOrderDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<Order>().Exists(id);

            if (exists)
            {
                var spec = new OrderWithCustomerSpec(id);
                var order = await _uof.GetRepository<Order>().FindSpec(spec);

                if (order == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadOrderDto>(order));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [HttpGet("GetOrderById")]
        public async Task<ActionResult<ReadOrderDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<Order>().Exists(id);

            if (exists)
            {
                var order = await _uof.GetRepository<Order>().GetByIdAsync(id);

                if (order == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadOrderDto>(order));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }



        [HttpGet("SearchOrderWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadOrderDto>>> SearchOrderByCriteria(int? orderId = null, int? customerId = null, string customerName = null, decimal? totalPrice = null, DateTime? date = null)
        {
            var result = await _orderServices.SearchOrders(orderId, customerId, customerName, totalPrice, date);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost("InsertOrder")]
        public async Task<ActionResult> InsertOrderAsync(CreateOrderDto createOrder)
        {
            if (!_numbersValidator.IsValidInt(createOrder.CustomerId))
                return NotFound(new ApiResponse(404, AppMessages.INVALID_CUSTOMER));
            if (!_numbersValidator.IsValidDecimal(createOrder.TotalPrice))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PRICE));

            var orderBook = _mapper.Map<CreateOrderDto, Order>(createOrder);
            _uof.GetRepository<Order>().InsertAsync(orderBook);
            await _uof.Commit();

            return Ok(new ApiResponse(201, AppMessages.INSERTED));
        }

        [HttpPost("InsertBookOrder")]
        public async Task<ActionResult> InsertOrderBookAsync(CreateBookOrderDetailsDto createOrderBooks)
        {
            if (!_numbersValidator.IsValidInt(createOrderBooks.OrderId))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_ORDER));
            if (!_numbersValidator.IsValidInt(createOrderBooks.BookId))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_BOOK));
            if (!_numbersValidator.IsValidDecimal(createOrderBooks.Price))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PRICE));
            if (!_numbersValidator.IsValidInt(createOrderBooks.Quantity))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_QUANTITY));

            var validOrderId = await _orderServices.IsValidOrderId(createOrderBooks.OrderId);

            if (validOrderId)
            {
                var validBook = await _orderServices.IsAvailableBook(createOrderBooks.BookId, createOrderBooks.Quantity);
                if (validBook)
                {
                    var orderBook = _mapper.Map<CreateBookOrderDetailsDto, BookOrderDetails>(createOrderBooks);
                    _uof.GetRepository<BookOrderDetails>().InsertAsync(orderBook);
                    await _uof.Commit();
                    _orderServices.DecreaseQuantity(createOrderBooks.BookId, createOrderBooks.Quantity);
                    _orderServices.AddAuthorProfits(createOrderBooks.BookId, createOrderBooks.Quantity, createOrderBooks.Price);
                    return Ok(new ApiResponse(201, AppMessages.INSERTED));
                }
                else
                {
                    return NotFound(new ApiResponse(404, AppMessages.UNAVAILABLE_BOOK));
                }
            }
            else
            {
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ORDER));
            }
        }
        #endregion

    }
}
