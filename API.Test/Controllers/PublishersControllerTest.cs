using API.Controllers;
using Application.DTOs.Author;
using Application;
using Application.Interfaces;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Specifications.BookSpec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using IMapper = AutoMapper.IMapper;
using Application.Handlers;
using Domain.Constants;
using Application.DTOs.Publisher;
using Infrastructure.Specifications.PublisherSpec;

namespace API.Test
{
    public class PublishersControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<PublishersController>> _loggerMock;
        private readonly PublishersController _publishersController;
        private readonly Mock<IPhoneNumberValidator> _phoneNumberValidatorMock;
        private readonly Mock<IPublisherServices> _publisherServicesMock;
        private readonly Mock<IGenericRepository<Publisher>> _publisherRepositoryMock;

        public PublishersControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _phoneNumberValidatorMock = new Mock<IPhoneNumberValidator>();
            _publisherServicesMock = new Mock<IPublisherServices>();
            _loggerMock = new Mock<ILogger<PublishersController>>();
            _publisherRepositoryMock = new Mock<IGenericRepository<Publisher>>();
            _publishersController = new PublishersController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _phoneNumberValidatorMock.Object,
                                                        _publisherServicesMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        [Fact]
        public async Task GetAllPublishers()
        {
            // Arrange
            var pageSize = 6;
            var pageIndex = 1;
            var isPagingEnabled = true;


            var publishers = GetPublishersData();

            var readPublisherDtos = GetReadPublishersDtoData();

            _publisherRepositoryMock.Setup(repo => repo.FindAllSpec(It.IsAny<PublisherSpec>()))
               .ReturnsAsync(publishers);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Publisher>())
                    .Returns(_publisherRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadPublisherDto>>(publishers)).Returns(readPublisherDtos).Verifiable();
            // Act
            var result = await _publishersController.GetAllPublishersAsync(pageSize, pageIndex, isPagingEnabled);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Pagination<ReadPublisherDto>>>(result);
        }

        #endregion

        #region GetById
        [Fact]
        public async Task GetPublisherById_ReturnsPublisher()
        {
            //Arrange
            var publishers = GetPublishersData();
            int valid_publisherId = 1;
            int invalid_publisherId = 200;
            var validPublisher = publishers[0];
            var validReadpublisherDto = new ReadPublisherDto
            {
                Id = validPublisher.Id,
                PublisherName = validPublisher.PublisherName,
                PublisherPhoneNumber = validPublisher.PublisherPhoneNumber
            };

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Publisher>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => publishers.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Publisher>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => publishers.Find(a => a.Id == id))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Publisher, ReadPublisherDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Publisher, ReadPublisherDto>(It.IsAny<Publisher>()))
                .Returns((Publisher publisher) => mapper.Map<ReadPublisherDto>(publisher))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _publishersController.GetPublisherByIdAsync(valid_publisherId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedPublisher = validOkResult.Value as ReadPublisherDto;

            // Act - Invalid ID
            var invalidActionResult = await _publishersController.GetPublisherByIdAsync(invalid_publisherId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedPublisher);
            Assert.Equal(validPublisher.Id, validReturnedPublisher.Id);
            Assert.Equal(validPublisher.PublisherName, validReturnedPublisher.PublisherName);
            Assert.Equal(validPublisher.PublisherPhoneNumber, validReturnedPublisher.PublisherPhoneNumber);

            // Assert - Invalid ID
            Assert.NotNull(invalidNotFoundResult);
            Assert.NotNull(invalidApiResponse);
            Assert.Equal(404, invalidNotFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidApiResponse.Message);

            // Verify the interactions with the mocks
            _unitOfWorkMock.Verify();
            _mapperMock.Verify();
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertPublisherAsync()
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                PublisherName = "Ahmed",
                PublisherPhoneNumber = "01013451699"
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

            _mapperMock.Setup(m => m.Map<CreatePublisherDto, Publisher>(It.IsAny<CreatePublisherDto>()))
                .Returns((CreatePublisherDto dto) => new Publisher
                {
                    PublisherName = dto.PublisherName,
                    PublisherPhoneNumber = dto.PublisherPhoneNumber
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Publisher>().InsertAsync(It.IsAny<Publisher>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                        .Returns(Task.FromResult(1));

            // Act
            var result = await _publishersController.InsertPublisherAsync(publisherDto);

            if (_phoneNumberValidatorMock.Invocations.Count == 1)
            {
                var okResult = result as OkObjectResult;
                var apiResponseOkResult = okResult.Value as ApiResponse;
                Assert.NotNull(result);
                Assert.NotNull(okResult);
                Assert.NotNull(apiResponseOkResult);
                Assert.Equal(201, apiResponseOkResult.StatusCode);
                Assert.Equal(AppMessages.INSERTED, apiResponseOkResult.Message);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadResult = badRequestResult.Value as ApiResponse;
                Assert.NotNull(result);
                Assert.NotNull(badRequestResult);
                Assert.NotNull(apiResponseBadResult);
                Assert.Equal(400, badRequestResult.StatusCode);
                Assert.Equal(AppMessages.INVALID_PHONENUMBER, apiResponseBadResult.Message);
            }

            // Verify the interactions with the mocks
            _phoneNumberValidatorMock.Verify();
            _mapperMock.Verify();
            _unitOfWorkMock.Verify();
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdatePublisherAsync()
        {
            // Arrange - Valid publisher
            var publisherDto = new ReadPublisherDto
            {
                Id = 1 ,
                PublisherName = "Ahmed",
                PublisherPhoneNumber = "01013451699"
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _mapperMock.Setup(m => m.Map<ReadPublisherDto, Publisher>(It.IsAny<ReadPublisherDto>()))
                .Returns((ReadPublisherDto dto) => new Publisher
                {
                    Id = dto.Id,
                    PublisherName = dto.PublisherName,
                    PublisherPhoneNumber = dto.PublisherPhoneNumber
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Publisher>().UpdateAsync(It.IsAny<Publisher>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid publisher
            var validResult = await _publishersController.UpdatePublisherAsync(publisherDto);

            // Assert - Valid publisher
            var okResult = validResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(validResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(AppMessages.UPDATED, apiResponseOkResult.Message);

            // Arrange - Invalid phone number
            var invalidPhoneNumberPublisherDto = new ReadPublisherDto
            {
                Id = 1,
                PublisherName = "Updated Author",
                PublisherPhoneNumber = "017890"
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var invalidPhoneNumberResult = await _publishersController.UpdatePublisherAsync(invalidPhoneNumberPublisherDto);

            // Assert - Invalid phone number
            var invalidPhoneNumberBadRequestResult = invalidPhoneNumberResult as BadRequestObjectResult;
            var invalidPhoneNumberApiResponse = invalidPhoneNumberBadRequestResult.Value as ApiResponse;

            Assert.NotNull(invalidPhoneNumberResult);
            Assert.NotNull(invalidPhoneNumberBadRequestResult);
            Assert.NotNull(invalidPhoneNumberApiResponse);
            Assert.Equal(400, invalidPhoneNumberApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidPhoneNumberApiResponse.Message);

            _unitOfWorkMock.Verify();
            _phoneNumberValidatorMock.Verify();
            _mapperMock.Verify();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeletePublisherAsync()
        {
            // Arrange
            int publisherId = 1;
            var bookRepositoryMock = new Mock<IGenericRepository<Book>>();
            var publisherRepositoryMock = new Mock<IGenericRepository<Publisher>>();

            var books = new List<Book>
            {
                new Book { Id = 1, BookTitle = "Book 1", PublisherId = publisherId },
                new Book { Id = 2, BookTitle = "Book 2", PublisherId = publisherId }
            };

            var publisher = new Publisher
            {
                Id = publisherId,
                PublisherName = "Rawan",
                PublisherPhoneNumber = "01013451622",
            };

            var booksWithAuthorAndPublisherSpec = new BooksWithAuthorAndPublisherSpec(null, publisherId, null);
            bookRepositoryMock.Setup(repo => repo.FindAllSpec(booksWithAuthorAndPublisherSpec))
                .ReturnsAsync(books)
                .Verifiable();

            publisherRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<PublisherSpec>()))
                .ReturnsAsync(publisher)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>())
                .Returns(bookRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Publisher>())
                .Returns(publisherRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _publishersController.DeletePublisherAsync(publisherId);

            //Assert 
            if (books.Count > 0)
            {
                var okResult = result as OkObjectResult;
                var apiResponseOkResult = okResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(okResult);
                Assert.NotNull(apiResponseOkResult);
                Assert.Equal(201, apiResponseOkResult.StatusCode);
                Assert.Equal(AppMessages.DELETED, apiResponseOkResult.Message);

                bookRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()), Times.Once);
                publisherRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<PublisherSpec>()), Times.Once);
                publisherRepositoryMock.Verify(repo => repo.DeleteAsync(publisher), Times.Once);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadRequest = badRequestResult.Value as ApiResponse;

                Assert.Equal(400, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.FAILED_DELETE, apiResponseBadRequest.Message);

                bookRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()), Times.Once);
                publisherRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<PublisherSpec>()), Times.Never);
                publisherRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Publisher>()), Times.Never);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
            }
        }
        #endregion

        #region Search
        [Fact]
        public async Task SearchPublisherWithCriteria()
        {
            // Arrange - Valid phone number
            string validName = "Rawan";
            string validPhone = "01013451622";

            var expectedResult = new List<ReadPublisherDto>
            {
                new ReadPublisherDto
                {
                    Id = 1,
                    PublisherName = validName,
                    PublisherPhoneNumber = validPhone,
                }
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _publisherServicesMock.Setup(services => services.SearchWithCriteria(validName, validPhone))
                .ReturnsAsync(expectedResult);

            // Act - Valid phone number
            var result = await _publishersController.SearchPublisherWithCriteria(validName, validPhone);

            // Assert - Valid phone number
            if (result == null)
            {
                var errorResult = result.Result as NotFoundObjectResult;
                var apiResponseBadRequest = errorResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(errorResult);
                Assert.NotNull(apiResponseBadRequest);
                Assert.Equal(404, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.INVALID_ID, apiResponseBadRequest.Message);
            }
            else
            {
                var successResult = result.Result as OkObjectResult;

                Assert.NotNull(result);
                Assert.NotNull(successResult);
            }

            // Arrange - Invalid phone number
            string InvalidPhone = "011622";

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var ErrorResult = await _publishersController.SearchPublisherWithCriteria(validName, InvalidPhone);

            // Assert - Invalid phone number
            var invalidPhoneNumberBadRequestResult = ErrorResult.Result as BadRequestObjectResult;
            var invalidPhoneNumberApiResponse = invalidPhoneNumberBadRequestResult.Value as ApiResponse;

            Assert.NotNull(ErrorResult);
            Assert.NotNull(invalidPhoneNumberBadRequestResult);
            Assert.NotNull(invalidPhoneNumberApiResponse);
            Assert.Equal(400, invalidPhoneNumberApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidPhoneNumberApiResponse.Message);
        }
        #endregion


        #region Data
        private List<Publisher> GetPublishersData()
        {
            List<Publisher> publishersData = new List<Publisher>
            {
                new Publisher
                {
                    Id = 1,
                    PublisherName = "Rawan",
                    PublisherPhoneNumber = "01013451622"
                },

                new Publisher
                {
                    Id = 2,
                    PublisherName = "Rita",
                    PublisherPhoneNumber = "01013451644"
                },

                new Publisher
                {
                    Id = 3,
                    PublisherName = "Ali",
                    PublisherPhoneNumber = "01055451699"
                },
            };

            return publishersData;
        }

        private IReadOnlyList<ReadPublisherDto> GetReadPublishersDtoData()
        {
            List<ReadPublisherDto> publishersDtoData = new List<ReadPublisherDto>
            {
                new ReadPublisherDto
                {
                    Id = 1,
                    PublisherName = "Rawan",
                    PublisherPhoneNumber = "01013451622"
                },

                new ReadPublisherDto
                {
                    Id = 2,
                    PublisherName = "Rita",
                    PublisherPhoneNumber = "01013451644"
                },

                new ReadPublisherDto
                {
                    Id = 3,
                    PublisherName = "Ali",
                    PublisherPhoneNumber = "01055451699"
                }
            };

            return publishersDtoData;
        }

        #endregion

    }
}