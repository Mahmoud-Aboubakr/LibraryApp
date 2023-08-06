using Application.DTOs.Book;
using Application.DTOs.Publisher;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.Specifications.BannedCustomerSpec;
using Infrastructure.Specifications.BookOrderDetailsSpec;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.CustomerSpec;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IBookServices _searchBookDataWithDetailService;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IUnitOfWork uof,
                               IMapper mapper,
                               IBookServices searchBookDataWithDetailService,
                               INumbersValidator numbersValidator,
                               ILogger<BooksController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _searchBookDataWithDetailService = searchBookDataWithDetailService;
            _numbersValidator = numbersValidator;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllBooksAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> GetAllBooksAsync()
        {
            var books = await _uof.GetRepository<Book>().GetAllAsync();
            if (books == null || books.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetAllBooksWithDetails")]
        public async Task<ActionResult<Pagination<ReadBookDto>>> GetAllBooksWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));
            }
            var spec = new BooksWithAuthorAndPublisherSpec(pagesize, pageindex, isPagingEnabled);
            var totalBooks = await _uof.GetRepository<Book>().CountAsync(spec);
            var books = await _uof.GetRepository<Book>().FindAllSpec(spec);
            var mappedbooks = _mapper.Map<IReadOnlyList<ReadBookDto>>(books);
            if (mappedbooks == null || totalBooks == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            var paginationData = new Pagination<ReadBookDto>(spec.PageIndex, spec.PageSize, totalBooks, mappedbooks);
            return Ok(paginationData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdAsync(string id)
        {
            var exists = await _uof.GetRepository<Book>().Exists(int.Parse(id));

            if (exists)
            {
                var book = await _uof.GetRepository<Book>().GetByIdAsync(int.Parse(id));

                if (book == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdWithDetailAsync(string id)
        {
            var exists = await _uof.GetRepository<Book>().Exists(int.Parse(id));

            if (exists)
            {
                var spec = new BooksWithAuthorAndPublisherSpec(int.Parse(id));
                var book = await _uof.GetRepository<Book>().FindSpec(spec);
                if (book == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }


        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> SearchByCriteria(string? bookTitle = null, string? authorName = null, string? publisherName = null)
        {
            var result = await _searchBookDataWithDetailService.SearchBookDataWithDetail(bookTitle, authorName, publisherName);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }

        #endregion

        #region POST
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertBookAsync(CreateBookDto insertBookDto)
        {
            if (!_numbersValidator.IsValidInt(insertBookDto.Quantity))
                return BadRequest(new ApiResponse(400 , AppMessages.INVALID_QUANTITY ));
            if (!_numbersValidator.IsValidDecimal(insertBookDto.Price))
                return BadRequest(new ApiResponse(404, AppMessages.INVALID_PRICE));
            var book = _mapper.Map<CreateBookDto, Book>(insertBookDto);
            _uof.GetRepository<Book>().InsertAsync(book);
            await _uof.Commit();

            return Ok(new ApiResponse(201));
        }
        #endregion

        #region PUT
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            var result = await _uof.GetRepository<Book>().Exists(updateBookDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_numbersValidator.IsValidInt(updateBookDto.Quantity))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_QUANTITY));
            if (!_numbersValidator.IsValidDecimal(updateBookDto.Price))
                return BadRequest(new ApiResponse(404, AppMessages.INVALID_PRICE));

            var book = _mapper.Map<UpdateBookDto, Book>(updateBookDto);
            _uof.GetRepository<Book>().UpdateAsync(book);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteBook")]
        public async Task<ActionResult> DeleteBookAsync(int id)
        {
            var bookOrderDetailsSpec = new BookOrderDetailsWithBookAndCustomerSpec(null, id);
            var result = _uof.GetRepository<BookOrderDetails>().FindAllSpec(bookOrderDetailsSpec).Result;
            if (result.Count() > 0)
                return BadRequest(new ApiResponse(400, AppMessages.FAILED_DELETE));
            else
            {
                var bookSpec = new BooksWithAuthorAndPublisherSpec(id);
                var book = _uof.GetRepository<Book>().FindSpec(bookSpec).Result;
                _uof.GetRepository<Book>().DeleteAsync(book);
                await _uof.Commit();
                return Ok(new ApiResponse(201,AppMessages.DELETED));
            }
        }
        #endregion

    }
}

