using Application.DTOs.BookOrderDetails;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.AppServices;
using Infrastructure.Specifications.BookOrderDetailsSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BookOrderDetailsController : ControllerBase
    {
        private readonly IUnitOfWork<BookOrderDetails> _uof;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BookOrderDetailsController> _logger;

        public BookOrderDetailsController(IUnitOfWork<BookOrderDetails> uof,
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

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetAllOrderBooksAsync()
        {
            var orderBooks = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var orderBooks = await _uof.GetRepository().GetByIdAsync(id);

                if (orderBooks == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetAllWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadBookOrderDetailsDto>>> GetAllBorrowsWithDetails()
        {
            var spec = new BookOrderDetailsWithBookAndCustomerDetails();
            var orderBooks = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<BookOrderDetails>, IEnumerable<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [HttpGet("GetByIdWithDetails")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var spec = new BookOrderDetailsWithBookAndCustomerDetails(id);
                var orderBooks = await _uof.GetRepository().FindSpec(spec);

                if (orderBooks == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
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
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderBookAsync(ReadBookOrderDetailsDto readOrderBooksDto)
        {
            var orderBook = _mapper.Map<ReadBookOrderDetailsDto, BookOrderDetails>(readOrderBooksDto);
            _uof.GetRepository().DeleteAsync(orderBook);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }

}
