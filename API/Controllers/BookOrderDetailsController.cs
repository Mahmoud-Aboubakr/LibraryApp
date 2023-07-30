using Application.DTOs.Attendance;
using Application.DTOs.BookOrderDetails;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.AppServices;
using Infrastructure.Specifications.AttendanceSpec;
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

        #region GET
        [HttpGet("GetAllBookOrderDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetAllBookOrderDetailsAsync()
        {
            var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [HttpGet("GetAllBookOrderDetailsWithDetails")]
        public async Task<ActionResult<Pagination<ReadBookOrderDetailsDto>>> GetAllBookOrderDetailsWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new BookOrderDetailsWithBookAndCustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalBookOrderDetails = await _uof.GetRepository<BookOrderDetails>().CountAsync(spec);

            var bookOrderDetails = await _uof.GetRepository<BookOrderDetails>().FindAllSpec(spec);

            var mappedbookOrderDetails = _mapper.Map<IReadOnlyList<ReadBookOrderDetailsDto>>(bookOrderDetails);

            var paginationData = new Pagination<ReadBookOrderDetailsDto>(spec.PageIndex, spec.PageSize, totalBookOrderDetails, mappedbookOrderDetails);

            return Ok(paginationData);
        }

        [HttpGet("GetBookOrderDetailsById")]
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
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetBookOrderDetailsByIdWithDetails")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<BookOrderDetails>().Exists(id);

            if (exists)
            {
                var spec = new BookOrderDetailsWithBookAndCustomerSpec(id);
                var orderBooks = await _uof.GetRepository<BookOrderDetails>().FindSpec(spec);

                if (orderBooks == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchInBookOrderDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> SearchBookOrderDetails(int? orderId = null, string customerName = null, string bookTitle = null)
        {
            var result = await _orderServices.SearchBookOrderDetails(orderId, customerName, bookTitle);
            return Ok(result);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteBookOrderDetails")]
        public async Task<ActionResult> DeleteOrderBookAsync(int id)
        {
            var bookOrderDetailsSpec = new BookOrderDetailsWithBookAndCustomerSpec(id);
            var bookOrderDetails = _uof.GetRepository<BookOrderDetails>().FindSpec(bookOrderDetailsSpec).Result;
            _uof.GetRepository<BookOrderDetails>().DeleteAsync(bookOrderDetails);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
        
    }

}
