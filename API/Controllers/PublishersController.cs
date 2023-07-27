using Application.DTOs.Book;
using Application.DTOs.Publisher;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.PublisherSpec;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberValidator _phoneNumberValidator;
        private readonly IPublisherServices _searchPublisherDataService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IUnitOfWork uof,
            IMapper mapper,
            IPhoneNumberValidator phoneNumberValidator,
            IPublisherServices searchPublisherDataService,
            ILogger<PublishersController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchPublisherDataService = searchPublisherDataService;
            _logger = logger;
        }

        #region Get
        [HttpGet("GetAll")]
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> GetAllPublishersAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            var spec = new PublisherSpec(pagesize, pageindex, isPagingEnabled);
            var totalPublishers = await _uof.GetRepository<Publisher>().CountAsync(spec);
            var publishers = await _uof.GetRepository<Publisher>().FindAllSpec(spec);
            var mappedPublishers = _mapper.Map<IReadOnlyList<ReadPublisherDto>>(publishers);
            var paginationData = new Pagination<ReadPublisherDto>(spec.PageIndex, spec.PageSize, totalPublishers, mappedPublishers);
            return Ok(paginationData);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetPublisherByIdAsync(int id)
        {
            if (await _uof.GetRepository<Publisher>().Exists(id))
            {
                var publisher = await _uof.GetRepository<Publisher>().GetByIdAsync(id);
                return Ok(_mapper.Map<Publisher, ReadPublisherDto>(publisher));
            }

            return NotFound(new { Detail = $"{AppMessages.INVALID_ID} {id}" });
        }

        [HttpGet("SearchWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> SearchWithCriteria(string name = null, string phone = null)
        {
            var result = await _searchPublisherDataService.SearchWithCriteria(name, phone);
            return Ok(result);
        }
        #endregion

        #region Post
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertPublisherAsync(CreatePublisherDto createPublisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createPublisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<CreatePublisherDto, Publisher>(createPublisherDto);
                _uof.GetRepository<Publisher>().InsertAsync(publisher);
                await _uof.Commit();

                return StatusCode(201, AppMessages.INSERTED);
            }
            else
            {
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {createPublisherDto.PublisherPhoneNumber}" });
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<ActionResult> UpdatePublisherAsync(ReadPublisherDto publisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(publisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<ReadPublisherDto, Publisher>(publisherDto);
                _uof.GetRepository<Publisher>().UpdateAsync(publisher);
                await _uof.Commit();

                return Ok(AppMessages.UPDATED);
            }
            else
            {
                return BadRequest(new { Detail = $"{AppMessages.INVALID_PHONENUMBER} {publisherDto.PublisherPhoneNumber}" });
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<ActionResult> DeletePublisherAsync(int id)
        {
            var bookSpec = new BooksWithAuthorAndPublisherSpec(null,null,id);
            var result = _uof.GetRepository<Book>().FindAllSpec(bookSpec).Result;
            if (result.Count() > 0)
                return BadRequest(AppMessages.FAILED_DELETE);
            else
            {
                var publisherSpec = new PublisherSpec(id);
                var publisher = _uof.GetRepository<Publisher>().FindSpec(publisherSpec).Result;
                _uof.GetRepository<Publisher>().DeleteAsync(publisher);
                await _uof.Commit();
                return Ok(AppMessages.DELETED);
            }
        }
        #endregion
    }
}
