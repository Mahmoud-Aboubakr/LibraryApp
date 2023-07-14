using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IGenericRepository<Author> _authorRepo;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly ISearchAuthorDataService _searchAuthorDataService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IGenericRepository<Author> authorRepo,
                                  IMapper mapper,
                                  IPhoneNumberValidator phoneNumberValidator,
                                  ISearchAuthorDataService searchAuthorDataService,
                                  ILogger<AuthorsController> logger)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchAuthorDataService = searchAuthorDataService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadAuthorDto>>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepo.GetAllListAsync();
            return Ok(_mapper.Map<IReadOnlyList<Author>, IReadOnlyList<ReadAuthorDto>>(authors));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorByIdAsync(int id)
        {
            if(await _authorRepo.Exists(id))
            {
                var author = await _authorRepo.GetByIdAsync(id);
                return Ok(_mapper.Map<Author, ReadAuthorDto>(author));
            }

            return BadRequest(new { Detail = $"This is invalid Id" });
        }


        [HttpPost]
        public async Task<ActionResult> InsertAuthorAsync(CreateAuthorDto authorDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(authorDto.AuthorPhoneNumber))
            {
                var author = _mapper.Map<CreateAuthorDto, Author>(authorDto);
                _authorRepo.InsertAsync(author);
                await _authorRepo.SaveChangesAsync();

                return Ok(_mapper.Map<Author, CreateAuthorDto>(author));
            }
            else
            {
                return BadRequest(new { Detail = $"This is invalid phone number {authorDto.AuthorPhoneNumber}" });
            }
        }


        [HttpPut]
        public async Task<ActionResult> UpdateAuthorAsync(ReadAuthorDto authorDto)
        {
            var author = _mapper.Map<ReadAuthorDto, Author>(authorDto);
            _authorRepo.UpdateAsync(author);
            await _authorRepo.SaveChangesAsync();

            return Ok(_mapper.Map<Author, ReadAuthorDto>(author));
        }


        [HttpDelete]
        public async Task DeleteAuthorAsync(ReadAuthorDto authorDto)
        {
            var author = _mapper.Map<ReadAuthorDto, Author>(authorDto);
            _authorRepo.DeleteAsync(author);
            await _authorRepo.SaveChangesAsync();
        }

        [HttpGet("SearchWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadAuthorDto>>> SearchWithCriteria(string? name = null, string? phone = null)
        {
            var result = await _searchAuthorDataService.SearchWithCriteria(name, phone);
            return Ok(result);
        }
    }
}
