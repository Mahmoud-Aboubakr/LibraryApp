﻿using Application.DTOs.Attendance;
using Application.DTOs.Borrow;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Validators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Specifications.AttendanceSpec;
using Infrastructure.Specifications.BorrowSpec;
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

        #region GET
        [HttpGet("GetAllBorrows")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> GetAllBorrowsAsync()
        {
            var borrows = await _uof.GetRepository<Borrow>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Borrow>, IReadOnlyList<ReadBorrowDto>>(borrows));
        }

        [HttpGet("GetAllBorrowsWithDetails")]
        public async Task<ActionResult<Pagination<ReadBorrowDto>>> GetAllBorrowsWithDetails(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new BorrowWithBookAndCustomerSpec(pagesize, pageindex, isPagingEnabled);

            var totalBorrows = await _uof.GetRepository<Borrow>().CountAsync(spec);

            var borrows = await _uof.GetRepository<Borrow>().FindAllSpec(spec);

            var mappedborrows = _mapper.Map<IReadOnlyList<ReadBorrowDto>>(borrows);

            var paginationData = new Pagination<ReadBorrowDto>(spec.PageIndex, spec.PageSize, totalBorrows, mappedborrows);

            return Ok(paginationData);
        }

        [HttpGet("GetBorrowById")]
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
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("GetBorrowByIdWithDetails")]
        public async Task<ActionResult<ReadBorrowDto>> GetByIdWithIncludesAsync(int id)
        {
            var exists = await _uof.GetRepository<Borrow>().Exists(id);

            if (exists)
            {
                var spec = new BorrowWithBookAndCustomerSpec(id);
                var borrow = await _uof.GetRepository<Borrow>().FindSpec(spec);

                if (borrow == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadBorrowDto>(borrow));
            }
            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }


        [HttpGet("SearchBorrowsByCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadBorrowDto>>> SearchByCriteria(string customerName = null, string bookTitle = null, DateTime? date = null)
        {
            var result = await _borrowServices.SearchWithCriteria(customerName, bookTitle, date);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertBorrow")]
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
                    _uof.GetRepository<Borrow>().InsertAsync(borrow);
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
        [HttpDelete("DeleteBorrow")]
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
