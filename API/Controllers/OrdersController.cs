using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServices;
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

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IReadOnlyList<ReadOrderDto>>> GetAllOrdersWithDetails()
        {
            var orders = await _uof.GetRepository<Order>().GetAllListWithIncludesAsync(new Expression<Func<Order, object>>[] { x => x.Customer });
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<ReadOrderDto>>(orders));
        }

        [HttpGet("GetByIdWithIncludes")]
        public async Task<ActionResult<ReadOrderDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<Order>().Exists(id);

            if (exists)
            {
                var order = await _uof.GetRepository<Order>().GetByIdAsyncWithIncludes(id, new Expression<Func<Order, object>>[] { x => x.Customer });

                if (order == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadOrderDto>(order));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
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
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }



        [HttpGet("SearchOrderWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadOrderDto>>> SearchOrderByCriteria(int? orderId = null, int? customerId = null, string customerName = null, decimal? totalPrice = null, DateTime? date = null)
        {
            var result = await _orderServices.SearchOrders(orderId, customerId, customerName , totalPrice , date);
            return Ok(result);
        }

        //insert


        [HttpDelete("DeleteOrderAsync")]
        public void  DeletOrderAsync(int orderId)
        {
             _orderServices.DeletOrderAsync(orderId);           
        }



    }
}
