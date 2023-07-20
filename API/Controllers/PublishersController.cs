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
    public class PublishersController : ControllerBase
    {/*
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
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> GetAllPublishersAsync()
        {
            var publisher = await _uof.GetRepository<Publisher>().GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Publisher>, IReadOnlyList<ReadPublisherDto>>(publisher));
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
        public async Task<ActionResult> DeletePublisherAsync(ReadPublisherDto publisherDto)
        {
            var result = await _uof.GetRepository<Book>().FindAsync(b => b.PublisherId == publisherDto.Id);
            if (result != null)
                return BadRequest(AppMessages.FAILED_DELETE);
            else
            {
                var publisher = _mapper.Map<ReadPublisherDto, Publisher>(publisherDto);
                _uof.GetRepository<Publisher>().DeleteAsync(publisher);
                await _uof.Commit();
                return Ok(AppMessages.DELETED);
            }
        }
        #endregion
*/
    }
}
