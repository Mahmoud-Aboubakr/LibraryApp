﻿using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServicesContracts;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ISearchBookDataWithDetailService _searchBookDataWithDetailService;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IUnitOfWork uof,
                               IMapper mapper,
                               ISearchBookDataWithDetailService searchBookDataWithDetailService,
                               INumbersValidator numbersValidator,
                               ILogger<BooksController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _searchBookDataWithDetailService = searchBookDataWithDetailService;
            _numbersValidator = numbersValidator;
            _logger = logger;
        }

        [HttpGet("GetAllBooksAsync")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> GetAllBooksAsync()
        {
            var books = await _uof.GetRepository<Book>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetAllBooksWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> GetAllBooksWithDetails()
        {
            var books = await _uof.GetRepository<Book>().GetAllListWithIncludesAsync(new Expression<Func<Book, object>>[] { x => x.Author, x => x.Publisher });
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
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
            return BadRequest(new { Detail = $"this id : {id} is not vaild !!" });
        }

        [HttpGet("GetBookByIdWithDetailAsync")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdWithDetailAsync(int id)
        {
            var exists = await _uof.GetRepository<Book>().Exists(id);

            if (!exists)
            {
                var book = await _uof.GetRepository<Book>().GetByIdAsyncWithIncludes(id, new Expression<Func<Book, object>>[] { x => x.Author, x => x.Publisher });

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
            _uof.GetRepository<Book>().InsertAsync(book);
            await _uof.Commit();

            return Ok(_mapper.Map<Book, CreateBookDto>(book));
        }

        [HttpDelete]
        public async Task DeleteBookAsync(ReadBookDto readBookDto)
        {
            var book = _mapper.Map<ReadBookDto, Book>(readBookDto);
            _uof.GetRepository<Book>().DeleteAsync(book);
            await _uof.Commit();
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateBookAsync(UpdateBookDto updateBookDto)
        {
            if (!_numbersValidator.IsValidInt(updateBookDto.Quantity))
                return BadRequest(new { Detail = $"This is invalid quantity {updateBookDto.Quantity}" });
            if (!_numbersValidator.IsValidDecimal(updateBookDto.Price))
                return BadRequest(new { Detail = $"This is invalid price {updateBookDto.Price}" });

            var book = _mapper.Map<UpdateBookDto, Book>(updateBookDto);
            _uof.GetRepository<Book>().UpdateAsync(book);
            await _uof.Commit();

            return Ok(_mapper.Map<Book, UpdateBookDto>(book));
        }

        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBookDto>>> SearchByCriteria(string? bookTitle = null, string? authorName = null, string? publisherName = null)
        {
            var result = await _searchBookDataWithDetailService.SearchBookDataWithDetail(bookTitle, authorName, publisherName);
            return Ok(result);
        }
    }
}

