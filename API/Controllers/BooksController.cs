﻿using Application.DTOs.Book;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork<Book> _uof;
        private readonly IMapper _mapper;
        private readonly IBookServices _searchBookDataWithDetailService;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IUnitOfWork<Book> uof,
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
            var books = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Book>, IReadOnlyList<ReadBookDto>>(books));
        }

        [HttpGet("GetAllBooksWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadBookDto>>> GetAllBooksWithDetails()
        {
            var spec = new BooksWithAuthorAndPublisherSpec();
            var books = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<ReadBookDto>>(books));
        }

        [HttpGet("GetBookById")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdAsync(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var book = await _uof.GetRepository().GetByIdAsync(id);

                if (book == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBookDto>(book));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetBookByIdWithDetailAsync")]
        public async Task<ActionResult<ReadBookDto>> GetBookByIdWithDetailAsync(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var spec = new BooksWithAuthorAndPublisherSpec(id);
                var book = await _uof.GetRepository().FindSpec(spec);
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
            _uof.GetRepository().InsertAsync(book);
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
            _uof.GetRepository().UpdateAsync(book);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> DeleteBookAsync(ReadBookDto readBookDto)
        {
            var book = _mapper.Map<ReadBookDto, Book>(readBookDto);
            _uof.GetRepository().DeleteAsync(book);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion

    }
}

