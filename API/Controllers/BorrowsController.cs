using Application.DTOs.Attendance;
using Application.DTOs.Borrow;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BorrowSpec;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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

        #region GET
        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> GetAllBorrowsAsync()
        {
            var borrows = await _uof.GetRepository<Borrow>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(borrows));
        }
        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadBorrowDto>>> GetAllBorrowsWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
                return BadRequest(new ApiResponse(400 , AppMessages.INAVIL_PAGING));

            var spec = new BorrowWithBookAndCustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalBorrows = await _uof.GetRepository<Borrow>().CountAsync(spec);
            if (totalBorrows == 0)
                return NotFound(new ApiResponse(404 , AppMessages.NULL_DATA));

            var borrows = await _uof.GetRepository<Borrow>().FindAllSpec(spec);

            var mappedborrows = _mapper.Map<IReadOnlyList<ReadBorrowDto>>(borrows);

            var paginationData = new Pagination<ReadBorrowDto>(spec.Skip, spec.Take, totalBorrows, mappedborrows);

            return Ok(paginationData);
        }
        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var exists = await _uof.GetRepository<Borrow>().Exists(int.Parse(id));

            if (exists)
            {
                var borrow = await _uof.GetRepository<Borrow>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Borrow,ReadBorrowDto>(borrow));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }
        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdWithIncludesAsync(string id)
        {
            var exists = await _uof.GetRepository<Borrow>().Exists(int.Parse(id));

            if (exists)
            {
                var spec = new BorrowWithBookAndCustomerSpec(int.Parse(id));
                var borrow = await _uof.GetRepository<Borrow>().FindSpec(spec);
                return Ok(_mapper.Map<Borrow, ReadBorrowDto>(borrow));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager,Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> SearchByCriteria(string customerName = null, string bookTitle = null, DateTime? date = null)
        {
            var result = await _borrowServices.SearchWithCriteria(customerName, bookTitle, date);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404 , AppMessages.NOTFOUND_SEARCHDATA));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [Authorize(Roles = "Manager,Librarian")]
        [HttpPost]
        public async Task<ActionResult> InsertBorrowAsync(CreateBorrowDto borrowDto)
        {
            var banned = await _borrowServices.IsBannedCustomer(borrowDto.CustomerId);
            if (banned)
            {
                return BadRequest(new ApiResponse(400, AppMessages.BANNED_CUSTOMER));
            }
            else
            {
                if (!_borrowServices.CreateBorrowValidator(borrowDto.CustomerId))
                {
                    return BadRequest(new ApiResponse(400, AppMessages.MAX_BORROWING));                
                }
                else
                {
                    if (!_numbersValidator.IsValidInt(borrowDto.CustomerId))
                        return BadRequest(new ApiResponse(400, AppMessages.INVALID_CUSTOMER));
                    if (!_numbersValidator.IsValidInt(borrowDto.BookId))
                        return BadRequest(new ApiResponse(400, AppMessages.INVALID_BOOK));

                    var borrow = _mapper.Map<CreateBorrowDto, Borrow>(borrowDto);
                    _uof.GetRepository<Borrow>().InsertAsync(borrow);
                    await _uof.Commit();

                    return StatusCode(201);
                }
            }
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "Manager,Librarian")]
        [HttpDelete]
        public async Task<ActionResult> DeleteBorrowAsync(int id)
        {
            var borrowSpec = new BorrowWithBookAndCustomerSpec(id);
            var borrow = _uof.GetRepository<Borrow>().FindSpec(borrowSpec).Result;
            _uof.GetRepository<Borrow>().DeleteAsync(borrow);
            await _uof.Commit();
            return Ok(AppMessages.DELETED);
        }
        #endregion
        
    }
}
     