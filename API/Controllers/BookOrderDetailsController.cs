using Application.DTOs.Attendance;
using Application.DTOs.BookOrderDetails;
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
using Infrastructure.Specifications.BookOrderDetailsSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> GetAllBookOrderDetailsAsync()
        {
            var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetAllAsync();
            if (orderBooks == null || orderBooks.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<IReadOnlyList<BookOrderDetails>, IReadOnlyList<ReadBookOrderDetailsDto>>(orderBooks));
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadBookOrderDetailsDto>>> GetAllBookOrderDetailsWithIncludes(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));
            }
            var spec = new BookOrderDetailsWithBookAndCustomerSpec(pagesize, pageindex, isPagingEnabled);
            var totalBookOrderDetails = await _uof.GetRepository<BookOrderDetails>().CountAsync(spec);
            var bookOrderDetails = await _uof.GetRepository<BookOrderDetails>().FindAllSpec(spec);
            var mappedbookOrderDetails = _mapper.Map<IReadOnlyList<ReadBookOrderDetailsDto>>(bookOrderDetails);
            if (mappedbookOrderDetails == null || totalBookOrderDetails == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            var paginationData = new Pagination<ReadBookOrderDetailsDto>(spec.PageIndex, spec.PageSize, totalBookOrderDetails, mappedbookOrderDetails);
            return Ok(paginationData);
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetBookOrderDetailsById(string id)
        {
            var exists = await _uof.GetRepository<BookOrderDetails>().Exists(int.Parse(id));

            if (exists)
            {
                var orderBooks = await _uof.GetRepository<BookOrderDetails>().GetByIdAsync(int.Parse(id));

                if (orderBooks == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return NotFound(new ApiResponse(404 , AppMessages.INVALID_ID ));
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookOrderDetailsDto>> GetBookOrderDetailsByIdWithIncludesAsync(string id)
        {
            var exists = await _uof.GetRepository<BookOrderDetails>().Exists(int.Parse(id));

            if (exists)
            {
                var spec = new BookOrderDetailsWithBookAndCustomerSpec(int.Parse(id));
                var orderBooks = await _uof.GetRepository<BookOrderDetails>().FindSpec(spec);

                if (orderBooks == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBookOrderDetailsDto>(orderBooks));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadBookOrderDetailsDto>>> SearchBookOrderDetails(int? orderId = null, string customerName = null, string bookTitle = null)
        {
            var result = await _orderServices.SearchBookOrderDetails(orderId, customerName, bookTitle);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "Manager, Librarian")]
        [HttpDelete]
        public async Task<ActionResult> DeleteOrderBookDetailsAsync(int id)
        {
            var bookOrderDetailsSpec = new BookOrderDetailsWithBookAndCustomerSpec(id);
            var bookOrderDetails = _uof.GetRepository<BookOrderDetails>().FindSpec(bookOrderDetailsSpec).Result;
            _uof.GetRepository<BookOrderDetails>().DeleteAsync(bookOrderDetails);
            await _uof.Commit();
            return Ok(new ApiResponse(201, AppMessages.DELETED));
        }
        #endregion
        
    }

}
