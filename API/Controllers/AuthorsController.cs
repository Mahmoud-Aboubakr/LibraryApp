using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly IAuthorServices _searchAuthorDataService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IUnitOfWork uof,
            IMapper mapper,
            IPhoneNumberValidator phoneNumberValidator,
            IAuthorServices searchAuthorDataService,
            ILogger<AuthorsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchAuthorDataService = searchAuthorDataService;
            _logger = logger;
        }

        #region GET
        [HttpGet("GetAllAuthors")]
        public async Task<ActionResult<IReadOnlyList<ReadAuthorDto>>> GetAllAuthorsAsync()
        {
            var authors = await _uof.GetRepository<Author>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Author>, IReadOnlyList<ReadAuthorDto>>(authors));
        }

        [HttpGet("GetAuthorById")]
        public async Task<ActionResult> GetAuthorByIdAsync(int id)
        {
            if (await _uof.GetRepository<Author>().Exists(id))
            {
                var author = await _uof.GetRepository<Author>().GetByIdAsync(id);
                return Ok(_mapper.Map<Author, ReadAuthorDto>(author));
            }

            return NotFound(new { Detail = AppMessages.INVALID_ID });
        }


        [HttpGet("SearchAuthorWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadAuthorDto>>> SearchWithCriteria(string name = null, string phone = null)
        {
            var result = await _searchAuthorDataService.SearchWithCriteria(name, phone);
            return Ok(result);
        }
        #endregion

        #region POST
        [HttpPost("InsertAuthor")]
        public async Task<ActionResult> InsertAuthorAsync(CreateAuthorDto authorDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(authorDto.AuthorPhoneNumber))
            {
                var author = _mapper.Map<CreateAuthorDto, Author>(authorDto);
                _uof.GetRepository<Author>().InsertAsync(author);
                await _uof.Commit();

                return StatusCode(201, AppMessages.INSERTED);
            }
            else
            {
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {authorDto.AuthorPhoneNumber}" });
            }
        }
        #endregion

        #region PUT
        [HttpPut("UpdateAuthor")]
        public async Task<ActionResult> UpdateAuthorAsync(ReadAuthorDto authorDto)
        {
            var result = await _uof.GetRepository<Author>().Exists(authorDto.Id);
            if (!result)
                return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {authorDto.Id}" });
            if (!_phoneNumberValidator.IsValidPhoneNumber(authorDto.AuthorPhoneNumber))
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {authorDto.AuthorPhoneNumber}" });
            var author = _mapper.Map<ReadAuthorDto, Author>(authorDto);
            _uof.GetRepository<Author>().UpdateAsync(author);
            await _uof.Commit();

            return Ok(AppMessages.UPDATED);
        }
        #endregion

        #region DELETE
        [HttpDelete("DeleteAuthor")]
        public async Task<ActionResult> DeleteAuthorAsync(ReadAuthorDto authorDto)
        {
            var result = _uof.GetRepository<Book>().FindUsingWhereAsync(b => b.AuthorId == authorDto.Id);
            if (result == null)
                return BadRequest(AppMessages.FAILED_DELETE);
            else
            {
                var author = _mapper.Map<ReadAuthorDto, Author>(authorDto);
                _uof.GetRepository<Author>().DeleteAsync(author);
                await _uof.Commit();
                return Ok(AppMessages.DELETED);
            }
        }
        #endregion

    }
}
