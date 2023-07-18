using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IBorrowServices _borrowServices;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BorrowsController> _logger;

        public BorrowsController(IUnitOfWork uof,
            IMapper mapper,
            IBorrowServices borrowServices,
            INumbersValidator numbersValidator,
            ILogger<BorrowsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _borrowServices = borrowServices;
            _numbersValidator = numbersValidator;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> GetAllBorrowsAsync()
        {
            var borrows = await _uof.GetRepository<Borrow>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(borrows));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBorrowDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository<Borrow>().Exists(id);

            if (exists)
            {
                var borrow = await _uof.GetRepository<Borrow>().GetByIdAsync(id);

                if (borrow == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBorrowDto>(borrow));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpGet("GetAllWithDetails")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> GetAllBorrowsWithDetails()
        {
            var readBorrow = await _uof.GetRepository<Borrow>().GetAllListWithIncludesAsync(new Expression<Func<Borrow, object>>[] { x => x.Customer, x => x.Book });
            return Ok(_mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(readBorrow));
        }

        [HttpGet("GetByIdWithDetails")]
        public async Task<ActionResult<ReadBorrowDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<Borrow>().Exists(id);

            if (exists)
            {
                var readBorrow = await _uof.GetRepository<Borrow>().GetByIdAsyncWithIncludes(id, new Expression<Func<Borrow, object>>[] { x => x.Customer, x => x.Book });

                if (readBorrow == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBorrowDto>(readBorrow));
            }
            return BadRequest(new { Detail = $"Id : {id} is not vaild !!" });
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertBorrowAsync(CreateBorrowDto borrowDto)
        {
            var banned = await _borrowServices.IsBannedCustomer(borrowDto.CustomerId);
            if (banned)
            {
                return BadRequest(new { Detail = $"This Customer Is Banned " });

            }
            else
            {
                if (_borrowServices.CreateBorrowValidator(borrowDto.CustomerId))
                {
                    if (!_numbersValidator.IsValidInt(borrowDto.CustomerId))
                        return BadRequest(new { Detail = $"This is invalid CustomerId {borrowDto.CustomerId}" });
                    if (!_numbersValidator.IsValidInt(borrowDto.BookId))
                        return BadRequest(new { Detail = $"This is invalid BookId {borrowDto.BookId}" });

                    var borrow = _mapper.Map<CreateBorrowDto, Borrow>(borrowDto);
                    _uof.GetRepository<Borrow>().InsertAsync(borrow);
                    await _uof.Commit();

                    return Ok(_mapper.Map<Borrow, ReadBorrowDto>(borrow));
                }
                else
                {
                    return BadRequest(new { Detail = $"This Customer Borrowed 3 Books Today " });
                }
            }                
        }

        [HttpDelete]
        public async Task DeleteOrderBookAsync(ReadBorrowDto readBorrowDto)
        {
            var borrow = _mapper.Map<ReadBorrowDto, Borrow>(readBorrowDto);
            _uof.GetRepository<Borrow>().DeleteAsync(borrow);
            await _uof.Commit();
        }

        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> SearchByCriteria(string customerName = null, string bookTitle = null, DateTime? date = null)
        {
            var result = await _borrowServices.SearchWithCriteria(customerName, bookTitle, date);
            return Ok(result);
        }
    }
}
