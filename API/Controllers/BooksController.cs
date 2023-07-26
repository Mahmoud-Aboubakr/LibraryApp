using Application.DTOs.Book;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Specifications.BookSpec;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetAllBooksWithDetails")]
        public async Task<ActionResult<Pagination<ReadBookDto>>> GetAllBooksWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new BooksWithAuthorAndPublisherSpec(pagesize, pageindex, isPagingEnabled);

            var totalBooks = await _uof.GetRepository<Book>().CountAsync(spec);

            var books = await _uof.GetRepository<Book>().FindAllSpec(spec);

            var mappedbooks = _mapper.Map<IReadOnlyList<ReadBookDto>>(books);

            var paginationData = new Pagination<ReadBookDto>(spec.PageIndex, spec.PageSize, totalBooks, mappedbooks);

            return Ok(paginationData);
        }

        [HttpGet("GetBookById")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdAsync(int id)
        {
            var exists = await _uof.GetRepository<Book>().Exists(id);

            if (exists)
            {
                var book = await _uof.GetRepository<Book>().GetByIdAsync(id);

                if (book == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetBookByIdWithDetailAsync")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdWithDetailAsync(int id)
        {
            var exists = await _uof.GetRepository<Book>().Exists(id);

            if (exists)
            {
                var spec = new BooksWithAuthorAndPublisherSpec(id);
                var book = await _uof.GetRepository<Book>().FindSpec(spec);
                if (book == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> SearchByCriteria(string? bookTitle = null, string? authorName = null, string? publisherName = null)
        {
            var result = await _searchBookDataWithDetailService.SearchBookDataWithDetail(bookTitle, authorName, publisherName);
            return Ok(result);
        }

        #endregion

        #region POST
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertBookAsync(CreateBookDto insertBookDto)
        {
            if (!_numbersValidator.IsValidInt(insertBookDto.Quantity))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_QUANTITY} {insertBookDto.Quantity}" });
            if (!_numbersValidator.IsValidDecimal(insertBookDto.Price))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PRICE} {insertBookDto.Price}" });
            var book = _mapper.Map<CreateBookDto, Book>(insertBookDto);
            _uof.GetRepository<Book>().InsertAsync(book);
            await _uof.Commit();

            return StatusCode(201, AppMessages.INSERTED);
        }
        #endregion

        #region PUT
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            if (!_numbersValidator.IsValidInt(updateBookDto.Quantity))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_QUANTITY} {updateBookDto.Quantity}" });
            if (!_numbersValidator.IsValidDecimal(updateBookDto.Price))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PRICE} {updateBookDto.Price}" });

            var book = _mapper.Map<UpdateBookDto, Book>(updateBookDto);
            _uof.GetRepository<Book>().UpdateAsync(book);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> DeleteBookAsync(ReadBookDto readBookDto)
        {
            var book = _mapper.Map<ReadBookDto, Book>(readBookDto);
            _uof.GetRepository<Book>().DeleteAsync(book);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}

