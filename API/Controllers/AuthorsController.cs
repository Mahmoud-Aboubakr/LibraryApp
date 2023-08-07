using Application.DTOs.Author;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.Specifications.BookSpec;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly IAuthorServices _authorServices;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IUnitOfWork uof,
            IMapper mapper,
            IPhoneNumberValidator phoneNumberValidator,
            IAuthorServices authorServices,
            ILogger<AuthorsController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _authorServices = authorServices;
            _logger = logger;
        }

        #region GET
        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<Pagination<ReadAuthorDto>>> GetAllAuthorsAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
            {
                return BadRequest( new ApiResponse (400 ,AppMessages.INAVIL_PAGING));
            }
            var spec = new AuthorSpec(pagesize, pageindex, isPagingEnabled);
            var totalAuthors = await _uof.GetRepository<Author>().CountAsync(spec);
            var authors = await _uof.GetRepository<Author>().FindAllSpec(spec);
            var mappedauthors = _mapper.Map<IReadOnlyList<ReadAuthorDto>>(authors);
            if (mappedauthors == null || totalAuthors == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            var paginationData = new Pagination<ReadAuthorDto>(spec.PageIndex, spec.PageSize, totalAuthors, mappedauthors);
            return Ok(paginationData);
        }


        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet("{id}")]
       
        public async Task<ActionResult> GetAuthorByIdAsync(string id)
        {
            if (await _uof.GetRepository<Author>().Exists(int.Parse(id)))
            {
                var author = await _uof.GetRepository<Author>().GetByIdAsync(int.Parse(id));
                return Ok(_mapper.Map<Author, ReadAuthorDto>(author));
            }
            return NotFound(new ApiResponse(404 , AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadAuthorDto>>> SearchAuthorWithCriteria(string name = null, string phone = null)
        {
            if (!_phoneNumberValidator.IsValidPhoneNumber(phone))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var result = await _authorServices.SearchWithCriteria(name, phone);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region POST
        [Authorize(Roles = "Manager, Librarian")]
        [HttpPost]
        public async Task<ActionResult> InsertAuthorAsync(CreateAuthorDto authorDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(authorDto.AuthorPhoneNumber))
            {
                var author = _mapper.Map<CreateAuthorDto, Author>(authorDto);
                _uof.GetRepository<Author>().InsertAsync(author);
                await _uof.Commit();

                return Ok(new ApiResponse(201, AppMessages.INSERTED));
            }
            else
            {
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            }
        }
        #endregion

        #region PUT
        [Authorize(Roles = "Manager, Librarian")]
        [HttpPut]
        public async Task<ActionResult> UpdateAuthorAsync(UpdateAuthorDto authorDto)
        {
            var result = await _uof.GetRepository<Author>().Exists(authorDto.Id);
            if (!result)
                return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
            if (!_phoneNumberValidator.IsValidPhoneNumber(authorDto.AuthorPhoneNumber))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));

            var author = _mapper.Map<UpdateAuthorDto, Author>(authorDto);
            _uof.GetRepository<Author>().UpdateAsync(author);
            await _uof.Commit();

            return Ok(new ApiResponse(201, AppMessages.UPDATED));
        }
        #endregion

        #region DELETE
        [Authorize(Roles = "Manager, Librarian")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAuthorAsync(int id)
        {
            var bookSpec = new BooksWithAuthorAndPublisherSpec(null, id, null);
            var result = _uof.GetRepository<Book>().FindAllSpec(bookSpec).Result;
            if (result.Count() > 0)
                return BadRequest(new ApiResponse(400, AppMessages.FAILED_DELETE));
            else
            {
                var authorSpec = new AuthorSpec(id);
                var author = _uof.GetRepository<Author>().FindSpec(authorSpec).Result;
                _uof.GetRepository<Author>().DeleteAsync(author);
                await _uof.Commit();
                return Ok(new ApiResponse(201, AppMessages.DELETED));
            }
        }
        #endregion

    }
}
