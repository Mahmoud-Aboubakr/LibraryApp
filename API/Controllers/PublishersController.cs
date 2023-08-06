using Application.DTOs.Book;
using Application.DTOs.Publisher;
using Application.Handlers;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Application;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.PublisherSpec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> GetAllPublishersAsync(int pagesize = 6, int pageindex = 1, bool isPagingEnabled = true)
        {
            if (pagesize <= 0 || pageindex <= 0)
            {
                return BadRequest(new ApiResponse(400, AppMessages.INAVIL_PAGING));
            }
            var spec = new PublisherSpec(pagesize, pageindex, isPagingEnabled);
            var totalPublishers = await _uof.GetRepository<Publisher>().CountAsync(spec);
            var publishers = await _uof.GetRepository<Publisher>().FindAllSpec(spec);
            var mappedPublishers = _mapper.Map<IReadOnlyList<ReadPublisherDto>>(publishers);
            if (mappedPublishers == null || totalPublishers == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            var paginationData = new Pagination<ReadPublisherDto>(spec.PageIndex, spec.PageSize, totalPublishers, mappedPublishers);
            return Ok(paginationData);
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPublisherByIdAsync(int id)
        {
            if (await _uof.GetRepository<Publisher>().Exists(id))
            {
                var publisher = await _uof.GetRepository<Publisher>().GetByIdAsync(id);

                if (publisher == null)
                    return NotFound(new ApiResponse(404));

                return Ok(_mapper.Map<Publisher, ReadPublisherDto>(publisher));
            }
            return NotFound(new ApiResponse(404, AppMessages.INVALID_ID));
        }

        [Authorize(Roles = "Manager, Librarian")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> SearchPublisherWithCriteria(string name = null, string phone = null)
        {
            if (!_phoneNumberValidator.IsValidPhoneNumber(phone))
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            var result = await _searchPublisherDataService.SearchWithCriteria(name, phone);
            if (result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(result);
        }
        #endregion

        #region Post
        [Authorize(Roles = "Manager, Librarian")]
        [HttpPost]
        public async Task<ActionResult> InsertPublisherAsync(CreatePublisherDto createPublisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createPublisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<CreatePublisherDto, Publisher>(createPublisherDto);
                _uof.GetRepository<Publisher>().InsertAsync(publisher);
                await _uof.Commit();

                return Ok(new ApiResponse(201, AppMessages.INSERTED));
            }
            else
            {
                return BadRequest(new ApiResponse (400 , AppMessages.INVALID_PHONENUMBER));
            }
        }
        #endregion

        #region Put
        [Authorize(Roles = "Manager, Librarian")]
        [HttpPut]
        public async Task<ActionResult> UpdatePublisherAsync(ReadPublisherDto publisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(publisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<ReadPublisherDto, Publisher>(publisherDto);
                _uof.GetRepository<Publisher>().UpdateAsync(publisher);
                await _uof.Commit();

                return Ok( new ApiResponse(201,AppMessages.UPDATED));
            }
            else
            {
                return BadRequest(new ApiResponse(400, AppMessages.INVALID_PHONENUMBER));
            }
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Manager, Librarian")]
        [HttpDelete]
        public async Task<ActionResult> DeletePublisherAsync(int id)
        {
            var bookSpec = new BooksWithAuthorAndPublisherSpec(null,null,id);
            var result = _uof.GetRepository<Book>().FindAllSpec(bookSpec).Result;
            if (result.Count() > 0)
                return BadRequest(new ApiResponse(400,AppMessages.FAILED_DELETE));
            else
            {
                var publisherSpec = new PublisherSpec(id);
                var publisher = _uof.GetRepository<Publisher>().FindSpec(publisherSpec).Result;
                _uof.GetRepository<Publisher>().DeleteAsync(publisher);
                await _uof.Commit();
                return Ok(new ApiResponse(201, AppMessages.DELETED));
            }
        }
        #endregion

    }
}
