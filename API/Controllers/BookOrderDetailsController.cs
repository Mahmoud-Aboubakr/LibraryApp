using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
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
    public class BookOrderDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BookOrderDetailsController> _logger;

        public BookOrderDetailsController(IUnitOfWork uof,
            IMapper mapper,
            IOrderServices orderServices,
            INumbersValidator numbersValidator,
            ILogger<BookOrderDetailsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _orderServices = orderServices;
            _numbersValidator = numbersValidator;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetAllOrderBooksAsync()
        {
            var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<BookOrderDetails>().Exists(id);

            if (exists)
            {
                var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetByIdAsync(id);

                if (orderBooks == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpGet("GetAllWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetAllBorrowsWithDetails()
        {
            var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetAllListWithIncludesAsync(new Expression<Func<BookOrderDetails, object>>[] { x => x.Order.Customer, x => x.Book });
            return Ok(_mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [HttpGet("GetByIdWithDetails")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<BookOrderDetails>().Exists(id);

            if (exists)
            {
                var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetByIdAsyncWithIncludes(id, new Expression<Func<BookOrderDetails, object>>[] { x => x.Order.Customer });

                if (orderBooks == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpGet("GetBookOrderDetailsByOrderId")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetOrderByIdWithDetails(int orderId)
        {
            var result = await _orderServices.GetOrderByIdWithDetail(orderId);
            return Ok(result);
        }


        [HttpGet("SearchInBookOrderDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> SearchBookOrderDetails(int? orderId = null, string customerName = null, string bookTitle = null)
        {
            var result = await _orderServices.SearchBookOrderDetails(orderId, customerName, bookTitle);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertOrderBookAsync(CreateBookOrderDetailsDto createOrderBooks)
        {
            if (!_numbersValidator.IsValidInt(createOrderBooks.OrderId))
                return BadRequest(new { Detail = $"This is invalid OrderId {createOrderBooks.OrderId}" });
            if (!_numbersValidator.IsValidInt(createOrderBooks.BookId))
                return BadRequest(new { Detail = $"This is invalid BookId {createOrderBooks.BookId}" });
            if (!_numbersValidator.IsValidInt(createOrderBooks.Quantity))
                return BadRequest(new { Detail = $"This is invalid Quantity {createOrderBooks.Quantity}" });

            var orderBook = _mapper.Map<CreateBookOrderDetailsDto, BookOrderDetails>(createOrderBooks);
            _uof.GetRepository<BookOrderDetails>().InsertAsync(orderBook);
            await _uof.Commit();

            return Ok(_mapper.Map<BookOrderDetails, CreateBookOrderDetailsDto>(orderBook));
        }

        [HttpDelete]
        public async Task DeleteOrderBookAsync(ReadBookOrderDetailsDto readOrderBooksDto)
        {
            var orderBook = _mapper.Map<ReadBookOrderDetailsDto, BookOrderDetails>(readOrderBooksDto);
            _uof.GetRepository<BookOrderDetails>().DeleteAsync(orderBook);
            await _uof.Commit();
        }

     
    }

}
