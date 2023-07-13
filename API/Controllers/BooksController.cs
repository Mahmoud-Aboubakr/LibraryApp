using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Context;
using Persistence.Repositories;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IMapper _mapper;
        private readonly ISearchBookDataWithDetailService _searchBookDataWithDetailService;
        private readonly INumbersValidator _numbersValidator;
        private readonly LibraryDbContext context;

        public BooksController(IGenericRepository<Book> bookRepo,
                               IMapper mapper,
                               ISearchBookDataWithDetailService searchBookDataWithDetailService,
                               INumbersValidator numbersValidator,
                               LibraryDbContext context
                               )
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
            _searchBookDataWithDetailService = searchBookDataWithDetailService;
            _numbersValidator = numbersValidator;
            this.context = context;
        }

        [HttpGet("GetAllBooksAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetAllBooksWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> GetAllBooksWithDetails()
        {
            var books = await _bookRepo.GetAllListWithIncludesAsync(new Expression<Func<Book, object>>[] { x => x.Author, x => x.Publisher });
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetBookById")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdAsync(int id)
        {
            var exists = await _bookRepo.Exists(id);

            if (exists)
            {
                var book = await _bookRepo.GetByIdAsync(id);

                if (book == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return BadRequest(new { Detail = $"this id : {id} is not vaild !!" });
        }

        [HttpGet("GetBookByIdWithDetailAsync")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdWithDetailAsync(int id)
        {        
            var exists = await _bookRepo.Exists(id);

            if (!exists)
            {
                var book = await _bookRepo.GetByIdAsyncWithIncludes(id, new Expression<Func<Book, object>>[] { x => x.Author, x => x.Publisher });

                if (book == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return BadRequest("Invalid Id");          
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertBookAsync(CreateBookDto insertBookDto)
        {
            if (!_numbersValidator.IsValidInt(insertBookDto.Quantity))
                return BadRequest(new { Detail = $"This is invalid quantity {insertBookDto.Quantity}" });
            if (!_numbersValidator.IsValidDecimal(insertBookDto.Price))
                return BadRequest(new { Detail = $"This is invalid price {insertBookDto.Price}" });
            var book = _mapper.Map<CreateBookDto, Book>(insertBookDto);
            _bookRepo.InsertAsync(book);
            await _bookRepo.Complete();

            return Ok(_mapper.Map<Book, CreateBookDto>(book));
        }

        [HttpDelete]
        public async Task DeleteBookAsync(int id)
        {
            _bookRepo.DeleteByIdAsync(id);
            await _bookRepo.Complete();
        }


        [HttpPut("Update")]
        public async Task<ActionResult> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            var book = _mapper.Map<UpdateBookDto, Book>(updateBookDto);
            _bookRepo.UpdateAsync(book);
            await _bookRepo.Complete();

            return Ok(_mapper.Map<Book, UpdateBookDto>(book));
        }



        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> SearchByCriteria(string? bookTitle = null,string? authorName = null,string? publisherName = null)
        {
            var result = await _searchBookDataWithDetailService.SearchBookDataWithDetail(bookTitle,authorName,publisherName);
            return Ok(result);
        }

    }
}
