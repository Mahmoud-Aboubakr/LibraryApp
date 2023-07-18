using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.AppServices;
using Microsoft.AspNetCore.Http;
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
        private readonly ISearchPublisherDataService _searchPublisherDataService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IUnitOfWork uof,
            IMapper mapper,
            IPhoneNumberValidator phoneNumberValidator,
            ISearchPublisherDataService searchPublisherDataService,
            ILogger<PublishersController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _phoneNumberValidator = phoneNumberValidator;
            _searchPublisherDataService = searchPublisherDataService;
            _logger = logger;
        }

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

            return BadRequest(new { Detail = $"Invalid Id {id}" });
        }

        [HttpPost("Insert")]
        public async Task<ActionResult> InsertPublisherAsync(CreatePublisherDto createPublisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(createPublisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<CreatePublisherDto, Publisher>(createPublisherDto);
                _uof.GetRepository<Publisher>().InsertAsync(publisher);
                await _uof.Commit();

                return Ok(_mapper.Map<Publisher, ReadPublisherDto>(publisher));
            }
            else
            {
                return BadRequest(new { Detail = $"Invalid phone number {createPublisherDto.PublisherPhoneNumber}" });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePublisherAsync(ReadPublisherDto publisherDto)
        {
            if (_phoneNumberValidator.IsValidPhoneNumber(publisherDto.PublisherPhoneNumber))
            {
                var publisher = _mapper.Map<ReadPublisherDto, Publisher>(publisherDto);
                _uof.GetRepository<Publisher>().UpdateAsync(publisher);
                await _uof.Commit();

                return Ok(_mapper.Map<Publisher, ReadPublisherDto>(publisher));
            }
            else
            {
                return BadRequest(new { Detail = $"Invalid phone number {publisherDto.PublisherPhoneNumber}" });
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeletePublisherAsync(ReadPublisherDto publisherDto)
        {
            var result =await _uof.GetRepository<Book>().FindAsync(b => b.PublisherId == publisherDto.Id);
            if (result != null)
                return BadRequest("Can't delete this publisher because used in other tables");
            else
            {
                var publisher = _mapper.Map<ReadPublisherDto, Publisher>(publisherDto);
                _uof.GetRepository<Publisher>().DeleteAsync(publisher);
                await _uof.Commit();
                return Ok();
            }
        }

        [HttpGet("SearchWithCriteria")]
        public async Task<ActionResult<IReadOnlyList<ReadPublisherDto>>> SearchWithCriteria(string name = null, string phone = null)
        {
            var result = await _searchPublisherDataService.SearchWithCriteria(name, phone);
            return Ok(result);
        }
    }
}
