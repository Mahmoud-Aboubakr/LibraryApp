﻿using Application.DTOs.BookOrderDetails;
using Application.DTOs.Employee;
using Application.DTOs.Order;
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
            var spec = new OrderWithCustomerSpec(pagesize, pageindex, isPagingEnabled);
            var totalOrders = await _uof.GetRepository<Order>().CountAsync(spec);
            var orders = await _uof.GetRepository<Order>().FindAllSpec(spec);
            var mappedOrders = _mapper.Map<IReadOnlyList<ReadOrderDto>>(orders);
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
                    return NotFound();

                return Ok(_mapper.Map<ReadOrderDto>(order));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetOrderById")]
        public async Task<ActionResult<ReadOrderDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<Order>().Exists(id);

            if (exists)
            {
                var order = await _uof.GetRepository<Order>().GetByIdAsync(id);

                if (order == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadOrderDto>(order));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }



        [HttpGet("SearchOrderWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadOrderDto>>> SearchOrderByCriteria(int? orderId = null, int? customerId = null, string customerName = null, decimal? totalPrice = null, DateTime? date = null)
        {
            var result = await _orderServices.SearchOrders(orderId, customerId, customerName, totalPrice, date);
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost("InsertOrder")]
        public async Task<ActionResult> InsertOrderAsync(CreateOrderDto createOrder)
        {
            if (!_numbersValidator.IsValidInt(createOrder.CustomerId))
                return NotFound(new { Detail = $"{AppMessages.INVALID_CUSTOMER} {createOrder.CustomerId}" });
            if (!_numbersValidator.IsValidDecimal(createOrder.TotalPrice))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_QUANTITY} {createOrder.TotalPrice}" });

            var orderBook = _mapper.Map<CreateOrderDto, Order>(createOrder);
            _uof.GetRepository<Order>().InsertAsync(orderBook);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }

        [HttpPost("InsertBookOrder")]
        public async Task<ActionResult> InsertOrderBookAsync(CreateBookOrderDetailsDto createOrderBooks)
        {
            if (!_numbersValidator.IsValidInt(createOrderBooks.OrderId))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_ORDER} {createOrderBooks.OrderId}" });
            if (!_numbersValidator.IsValidInt(createOrderBooks.BookId))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_BOOK} {createOrderBooks.BookId}" });
            if (!_numbersValidator.IsValidDecimal(createOrderBooks.Price))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PRICE} {createOrderBooks.Price}" });
            if (!_numbersValidator.IsValidInt(createOrderBooks.Quantity))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_QUANTITY} {createOrderBooks.Quantity}" });

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
                    return StatusCode(201, AppMessages.INSERTED);
                }
                else
                {
                    return NotFound(new { Detail = AppMessages.UNAVAILABLE_BOOK });
                }
            }
            else
            {
                return NotFound(new { Detail = AppMessages.INVALID_ORDER });
            }
        }
        #endregion

        #region Delete
        [HttpDelete("DeleteOrderAsync")]
        public void DeletOrderAsync(int orderId)
        {
            _orderServices.DeletOrderAsync(orderId);
        }
        #endregion
    }
}
