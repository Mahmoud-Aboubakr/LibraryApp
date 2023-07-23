using Application.DTOs.Borrow;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Specifications;
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
        private readonly IUnitOfWork<Borrow> _uof;
        private readonly IMapper _mapper;
        private readonly IBorrowServices _borrowServices;
        private readonly INumbersValidator _numbersValidator;
        private readonly ILogger<BorrowsController> _logger;

        public BorrowsController(IUnitOfWork<Borrow>uof,
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

        #region GET
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> GetAllBorrowsAsync()
        {
            var borrows = await _uof.GetRepository().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(borrows));
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ReadBorrowDto>> GetById(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var borrow = await _uof.GetRepository().GetByIdAsync(id);

                if (borrow == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBorrowDto>(borrow));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetAllWithDetails")]
        public async Task<ActionResult<IEnumerable<ReadBorrowDto>>> GetAllBorrowsWithDetails()
        {
            var spec = new BorrowWithBookAndCustomerSpec();
            var borrows = await _uof.GetRepository().FindAllSpec(spec);
            return Ok(_mapper.Map<IEnumerable<Borrow>, IEnumerable<ReadBorrowDto>>(borrows));
        }

        [HttpGet("GetByIdWithDetails")]
        public async Task<ActionResult<ReadBorrowDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository().Exists(id);

            if (exists)
            {
                var spec = new BorrowWithBookAndCustomerSpec(id);
                var borrow = await _uof.GetRepository().FindSpec(spec);

                if (borrow == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBorrowDto>(borrow));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("SearchByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> SearchByCriteria(string customerName = null, string bookTitle = null, DateTime? date = null)
        {
            var result = await _borrowServices.SearchWithCriteria(customerName, bookTitle, date);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertBorrowAsync(CreateBorrowDto borrowDto)
        {
            var banned = await _borrowServices.IsBannedCustomer(borrowDto.CustomerId);
            if (banned)
            {
                return BadRequest(new { Detail = AppMessages.BANNED_CUSTOMER });

            }
            else
            {
                if (_borrowServices.CreateBorrowValidator(borrowDto.CustomerId))
                {
                    if (!_numbersValidator.IsValidInt(borrowDto.CustomerId))
                        return BadRequest(new { Detail = $"{AppMessages.INVALID_CUSTOMER} {borrowDto.CustomerId}" });
                    if (!_numbersValidator.IsValidInt(borrowDto.BookId))
                        return BadRequest(new { Detail = $"{AppMessages.INVALID_BOOK} {borrowDto.BookId}" });

                    var borrow = _mapper.Map<CreateBorrowDto, Borrow>(borrowDto);
                    _uof.GetRepository().InsertAsync(borrow);
                    await _uof.Commit();

                    return StatusCode(201, AppMessages.INSERTED);
                }
                else
                {
                    return BadRequest(new { Detail = AppMessages.MAX_BORROWING });
                }
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> DeleteOrderBookAsync(ReadBorrowDto readBorrowDto)
        {
            var borrow = _mapper.Map<ReadBorrowDto, Borrow>(readBorrowDto);
            _uof.GetRepository().DeleteAsync(borrow);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
    }
}
