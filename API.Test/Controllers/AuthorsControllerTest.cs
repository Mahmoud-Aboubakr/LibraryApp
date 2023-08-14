using API.Controllers;
using Application.DTOs.Author;
using Application.Handlers;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Interfaces;
using Application;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Specifications.BookSpec;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Test
{
    public class AuthorsControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AuthorsController>> _loggerMock;
        private readonly AuthorsController _authorsController;
        private readonly Mock<IPhoneNumberValidator> _phoneNumberValidatorMock;
        private readonly Mock<IAuthorServices> _authorServicesMock;
        private readonly Mock<IGenericRepository<Author>> _authorRepositoryMock;

        public AuthorsControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _phoneNumberValidatorMock = new Mock<IPhoneNumberValidator>();
            _authorServicesMock = new Mock<IAuthorServices>();
            _loggerMock = new Mock<ILogger<AuthorsController>>();
            _authorRepositoryMock = new Mock<IGenericRepository<Author>>();
            _authorsController = new AuthorsController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _phoneNumberValidatorMock.Object,
                                                        _authorServicesMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        [Fact]
        public async Task GetAllAuthors()
        {
            // Arrange
            var pageSize = 6;
            var pageIndex = 1;
            var isPagingEnabled = true;


            var authors = GetAuthorsData();

            var readAuthorDtos = GetReadAuthorsDtoData();

            _authorRepositoryMock.Setup(repo => repo.FindAllSpec(It.IsAny<AuthorSpec>()))
               .ReturnsAsync(authors);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Author>())
                    .Returns(_authorRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadAuthorDto>>(authors)).Returns(readAuthorDtos).Verifiable();
            // Act
            var result = await _authorsController.GetAllAuthorsAsync(pageSize, pageIndex, isPagingEnabled);
            //
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Pagination<ReadAuthorDto>>>(result);
        }

        #endregion

        #region GetById
        [Fact]
        public async Task GetAuthorById_ReturnsAuthor()
        {
            //Arrange
            var authors = GetAuthorsData();
            int valid_authorId = 1;
            int invalid_authorId = 200;
            var validAuthor = authors[0];
            var validReadAuthorDto = new ReadAuthorDto
            {
                Id = validAuthor.Id,
                AuthorName = validAuthor.AuthorName,
                AuthorPhoneNumber = validAuthor.AuthorPhoneNumber,
                AuthorProfits = (decimal)validAuthor.AuthorProfits
            };

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Author>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => authors.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Author>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => authors.Find(a => a.Id == id));
                //.Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, ReadAuthorDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Author, ReadAuthorDto>(It.IsAny<Author>()))
                .Returns((Author author) => mapper.Map<ReadAuthorDto>(author))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _authorsController.GetAuthorByIdAsync(valid_authorId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedAuthor = validOkResult.Value as ReadAuthorDto;

            // Act - Invalid ID
            var invalidActionResult = await _authorsController.GetAuthorByIdAsync(invalid_authorId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedAuthor);
            Assert.Equal(validAuthor.Id, validReturnedAuthor.Id);
            Assert.Equal(validAuthor.AuthorName, validReturnedAuthor.AuthorName);
            Assert.Equal(validAuthor.AuthorPhoneNumber, validReturnedAuthor.AuthorPhoneNumber);
            Assert.Equal(validAuthor.AuthorProfits, validReturnedAuthor.AuthorProfits);

            // Assert - Invalid ID
            Assert.NotNull(invalidNotFoundResult);
            Assert.NotNull(invalidApiResponse);
            Assert.Equal(404, invalidNotFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidApiResponse.Message);

            // Verify the interactions with the mocks
            //_unitOfWorkMock.Verify();
            //_mapperMock.Verify();
        }
        #endregion

        #region Search
        [Fact]
        public async Task SearchAuthorWithCriteria()
        {
            // Arrange - Valid phone number
            string validName = "Rawan";
            string validPhone = "01013451622";

            var expectedResult = new List<ReadAuthorDto>
            {
                new ReadAuthorDto
                {
                    Id = 1,
                    AuthorName = validName,
                    AuthorPhoneNumber = validPhone,
                    AuthorProfits = 500
                }
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _authorServicesMock.Setup(services => services.SearchWithCriteria(validName, validPhone))
                .ReturnsAsync(expectedResult);

            // Act - Valid phone number
            var result = await _authorsController.SearchAuthorWithCriteria(validName, validPhone);

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
            var ErrorResult = await _authorsController.SearchAuthorWithCriteria(validName, InvalidPhone);

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

        #region Insert
        [Fact]
        public async Task InsertAuthorAsync()
        {
            // Arrange
            var authorDto = new CreateAuthorDto
            {
                AuthorName = "Ahmed",
                AuthorPhoneNumber = "01013451699",
                AuthorProfits = 200
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

            _mapperMock.Setup(m => m.Map<CreateAuthorDto, Author>(It.IsAny<CreateAuthorDto>()))
                .Returns((CreateAuthorDto dto) => new Author
                {
                    AuthorName = dto.AuthorName,
                    AuthorPhoneNumber = dto.AuthorPhoneNumber,
                    AuthorProfits = dto.AuthorProfits
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Author>().InsertAsync(It.IsAny<Author>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                        .Returns(Task.FromResult(1));

            // Act
            var result = await _authorsController.InsertAuthorAsync(authorDto);

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
        public async Task UpdateAuthorAsync()
        {
            // Arrange - Valid Author
            var ValidAuthorDto = new UpdateAuthorDto
            {
                Id = 1,
                AuthorName = "Updated Author",
                AuthorPhoneNumber = "01234567890",
                AuthorProfits = 1000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Author>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _mapperMock.Setup(m => m.Map<UpdateAuthorDto, Author>(It.IsAny<UpdateAuthorDto>()))
                .Returns((UpdateAuthorDto dto) => new Author
                {
                    Id = dto.Id,
                    AuthorName = dto.AuthorName,
                    AuthorPhoneNumber = dto.AuthorPhoneNumber,
                    AuthorProfits = dto.AuthorProfits
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Author>().UpdateAsync(It.IsAny<Author>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid Author
            var validResult = await _authorsController.UpdateAuthorAsync(ValidAuthorDto);

            // Assert - Valid Author
            var okResult = validResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(validResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.UPDATED, apiResponseOkResult.Message);


            // Arrange - Invalid Author Id
            var InvalidAuthorDto = new UpdateAuthorDto
            {
                Id = 500,
                AuthorName = "Updated Author",
                AuthorPhoneNumber = "01234567890",
                AuthorProfits = 1000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Author>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .Verifiable();

            // Act - Invalid Author Id
            var invalidIdResult = await _authorsController.UpdateAuthorAsync(InvalidAuthorDto);

            // Assert - Invalid Author Id
            var invalidIdNotFoundResult = invalidIdResult as NotFoundObjectResult;
            var invalidIdApiResponse = invalidIdNotFoundResult.Value as ApiResponse;

            Assert.NotNull(invalidIdResult);
            Assert.NotNull(invalidIdNotFoundResult);
            Assert.NotNull(invalidIdApiResponse);
            Assert.Equal(404, invalidIdApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidIdApiResponse.Message);


            // Arrange - Invalid phone number
            var invalidPhoneNumberAuthorDto = new UpdateAuthorDto
            {
                Id = 1,
                AuthorName = "Updated Author",
                AuthorPhoneNumber = "017890",
                AuthorProfits = 1000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Author>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(true)
                    .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var invalidPhoneNumberResult = await _authorsController.UpdateAuthorAsync(invalidPhoneNumberAuthorDto);

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
        public async Task DeleteAuthorAsync()
        {
            // Arrange
            int authorId = 1;
            var bookRepositoryMock = new Mock<IGenericRepository<Book>>();
            var authorRepositoryMock = new Mock<IGenericRepository<Author>>();

            var books = new List<Book>
            {
                new Book { Id = 1, BookTitle = "Book 1", AuthorId = authorId },
                new Book { Id = 2, BookTitle = "Book 2", AuthorId = authorId }
            };

            var author = new Author
            {
                Id = authorId,
                AuthorName = "Rawan",
                AuthorPhoneNumber = "01013451622",
                AuthorProfits = 500
            };

            var booksWithAuthorAndPublisherSpec = new BooksWithAuthorAndPublisherSpec(null, authorId, null);
            bookRepositoryMock.Setup(repo => repo.FindAllSpec(booksWithAuthorAndPublisherSpec))
                .ReturnsAsync(books)
                .Verifiable();

            authorRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<AuthorSpec>()))
                .ReturnsAsync(author)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>())
                .Returns(bookRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Author>())
                .Returns(authorRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _authorsController.DeleteAuthorAsync(authorId);

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
                authorRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<AuthorSpec>()), Times.Once);
                authorRepositoryMock.Verify(repo => repo.DeleteAsync(author), Times.Once);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadRequest = badRequestResult.Value as ApiResponse;

                Assert.Equal(400, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.FAILED_DELETE, apiResponseBadRequest.Message);

                bookRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()), Times.Once);
                authorRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<AuthorSpec>()), Times.Never);
                authorRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Author>()), Times.Never);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
            }
        }
        #endregion


        #region Data
        private List<Author> GetAuthorsData()
        {
            List<Author> authorsData = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    AuthorName = "Rawan",
                    AuthorPhoneNumber = "01013451622",
                    AuthorProfits = 500
                },

                new Author
                {
                    Id = 2,
                    AuthorName = "Rita",
                    AuthorPhoneNumber = "01013451644",
                    AuthorProfits = 400
                },

                new Author
                {
                    Id = 3,
                    AuthorName = "Ali",
                    AuthorPhoneNumber = "01055451699",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 4,
                    AuthorName = "Ahmed",
                    AuthorPhoneNumber = "01055451779",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 5,
                    AuthorName = "Essam",
                    AuthorPhoneNumber = "01055888699",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 6,
                    AuthorName = "Hosam",
                    AuthorPhoneNumber = "01053351699",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 7,
                    AuthorName = "Mohamed",
                    AuthorPhoneNumber = "01055452299",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 8,
                    AuthorName = "Hana",
                    AuthorPhoneNumber = "01055451889",
                    AuthorProfits = 600
                },

                new Author
                {
                    Id = 9,
                    AuthorName = "Nada",
                    AuthorPhoneNumber = "01055454499",
                    AuthorProfits = 600
                }
            };

            return authorsData;
        }

        private IReadOnlyList<ReadAuthorDto> GetReadAuthorsDtoData()
        {
            List<ReadAuthorDto> authorsDtoData = new List<ReadAuthorDto>
            {
                new ReadAuthorDto
                {
                    Id = 1,
                    AuthorName = "Rawan",
                    AuthorPhoneNumber = "01013451622",
                    AuthorProfits = 500
                },

                new ReadAuthorDto
                {
                    Id = 2,
                    AuthorName = "Rita",
                    AuthorPhoneNumber = "01013451644",
                    AuthorProfits = 400
                },

                new ReadAuthorDto
                {
                    Id = 3,
                    AuthorName = "Ali",
                    AuthorPhoneNumber = "01055451699",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 4,
                    AuthorName = "Ahmed",
                    AuthorPhoneNumber = "01055451779",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 5,
                    AuthorName = "Essam",
                    AuthorPhoneNumber = "01055888699",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 6,
                    AuthorName = "Hosam",
                    AuthorPhoneNumber = "01053351699",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 7,
                    AuthorName = "Mohamed",
                    AuthorPhoneNumber = "01055452299",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 8,
                    AuthorName = "Hana",
                    AuthorPhoneNumber = "01055451889",
                    AuthorProfits = 600
                },

                new ReadAuthorDto
                {
                    Id = 9,
                    AuthorName = "Nada",
                    AuthorPhoneNumber = "01055454499",
                    AuthorProfits = 600
                }
            };

            return authorsDtoData;
        }

        #endregion

    }
}
