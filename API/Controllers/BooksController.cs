using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Context;
using Persistence.Repositories;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IMapper _mapper;

        public BooksController(IGenericRepository<Book> bookRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAllBooks()
        {
            var books = await _bookRepo.GetAllListWithIncludesAsync(new Expression<Func<Book, object>>[] { x => x.Author });
            return Ok( _mapper.Map<IReadOnlyList<Book>, IReadOnlyList<BookDto>>(books));
        }
    }
}
