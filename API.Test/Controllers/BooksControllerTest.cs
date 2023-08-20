using API.Controllers;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs.Author;
using Application.DTOs.Book;
using Application;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Application.Handlers;
using Domain.Constants;
using Infrastructure.Specifications.BookSpec;
using Application.DTOs.Borrow;
using Application.DTOs.Employee;
using Infrastructure.Specifications.BookOrderDetailsSpec;
using Infrastructure.Specifications.BorrowSpec;

namespace API.Test.Controllers
{
    public class BooksControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BooksController>> _loggerMock;
        private readonly BooksController _booksController;
        private readonly Mock<INumbersValidator> _numbersValidatorMock;
        private readonly Mock<IBookServices> _bookServicesMock;
        private readonly Mock<IGenericRepository<Book>> _bookRepositoryMock;

        public BooksControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _numbersValidatorMock = new Mock<INumbersValidator>();
            _bookServicesMock = new Mock<IBookServices>();
            _loggerMock = new Mock<ILogger<BooksController>>();
            _bookRepositoryMock = new Mock<IGenericRepository<Book>>();
            _booksController = new BooksController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _bookServicesMock.Object,
                                                        _numbersValidatorMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        [Fact]
        public async Task GetAllBooks()
        {
            // Arrange
            var books = GetBooksData();

            var readBookDtos = GetReadBooksDtoData();

            _bookRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>()).Returns(_bookRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadBookDto>>(books)).Returns(readBookDtos).Verifiable();

            // Act
            var result = await _booksController.GetAllBooksAsync();

            //Assert
            if (books != null)
            {
                Assert.NotNull(result);
                Assert.IsType<ActionResult<IReadOnlyList<ReadBookDto>>>(result);
            }
            else
            {
                var invalidNotFoundResult = result.Value as NotFoundObjectResult;
                var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;
                Assert.NotNull(invalidNotFoundResult);
                Assert.NotNull(invalidApiResponse);
                Assert.Equal(404, invalidNotFoundResult.StatusCode);
            }
        }

        #endregion

        #region GetAllWithDetails
        [Fact]
        public async Task GetAllBooksWithDetails()
        {
            // Arrange
            var pageSize = 6;
            var pageIndex = 1;
            var isPagingEnabled = true;

            var books = GetBooksData();

            var readBookDtos = GetReadBooksDtoData();

            _bookRepositoryMock.Setup(repo => repo.FindAllSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()))
               .ReturnsAsync(books);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>())
                    .Returns(_bookRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadBookDto>>(books)).Returns(readBookDtos).Verifiable();

            // Act
            var result = await _booksController.GetAllBooksWithDetails(pageSize, pageIndex, isPagingEnabled);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Pagination<ReadBookDto>>>(result);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetBookById()
        {
            //Arrange
            var books = GetBooksData();
            int valid_bookId = 1;
            int invalid_bookId = 200;
            var validBook = books[0];

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => books.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => books.Find(a => a.Id == id))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, ReadBookDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Book, ReadBookDto>(It.IsAny<Book>()))
                .Returns((Book book) => mapper.Map<ReadBookDto>(book))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _booksController.GetBookByIdAsync(valid_bookId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedBook = validOkResult.Value as ReadBookDto;

            // Act - Invalid ID
            var invalidActionResult = await _booksController.GetBookByIdAsync(invalid_bookId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedBook);
            Assert.Equal(validBook.Id, validReturnedBook.Id);
            Assert.Equal(validBook.BookTitle, validReturnedBook.BookTitle);
            Assert.Equal(validBook.Quantity, validReturnedBook.Quantity);
            Assert.Equal(validBook.Price, validReturnedBook.Price);
            Assert.Equal(validBook.AuthorId, validReturnedBook.AuthorId);
            Assert.Equal(validBook.PublisherId, validReturnedBook.PublisherId);

            // Assert - Invalid ID
            Assert.NotNull(invalidNotFoundResult);
            Assert.NotNull(invalidApiResponse);
            Assert.Equal(404, invalidNotFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidApiResponse.Message);

        }
        #endregion

        #region GetBookByIdWithDetail
        [Fact]
        public async Task GetByIdWithIncludesAsync_Ok()
        {
            // Arrange
            var id = "1";
            var book = new Book
            {
                Id = 1,
                BookTitle = "BookOne",
                Quantity = 10,
                Price = 500,
                AuthorId = 1,
                PublisherId = 1
            };
            var bookDto = new ReadBookDto
            {
                Id = 1,
                BookTitle = "BookOne",
                Quantity = 10,
                Price = 500,
                AuthorId = 1,
                PublisherId = 1
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Exists(int.Parse(id))).ReturnsAsync(true);
            var spec = new BooksWithAuthorAndPublisherSpec(int.Parse(id));
            _unitOfWorkMock.Setup(u => u.GetRepository<Book>().FindSpec(spec)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<Book, ReadBookDto>(book)).Returns(bookDto);

            // Act
            var result = await _booksController.GetBookByIdWithDetailAsync(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var successResult = result as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(successResult);
        }

        [Fact]
        public async Task GetByIdWithIncludesAsync_NotFound()
        {
            // Arrange
            var id = "100";
            var book = new Book
            {
                Id = 100,
                BookTitle = "BookOne",
                Quantity = 10,
                Price = 500,
                AuthorId = 1,
                PublisherId = 1
            };
            var bookDto = new ReadBookDto
            {
                Id = 100,
                BookTitle = "BookOne",
                Quantity = 10,
                Price = 500,
                AuthorId = 1,
                PublisherId = 1
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Exists(int.Parse(id))).ReturnsAsync(false);

            // Act
            var result = await _booksController.GetBookByIdWithDetailAsync(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, (notFoundResult.Value as ApiResponse).Message);
        }
        #endregion

        #region Search
        [Fact]
        public async Task SearchBookWithCriteria()
        {
            // Arrange
            string bookTitle = "BookOne";
            string authorName = "AuthorOne";
            string publisherName = "PublisgerOne";

            var expectedResult = new List<ReadBookDto>
            {
                new ReadBookDto
                {
                    Id = 1,
                    BookTitle = bookTitle,
                    Quantity = 10,
                    Price = 500,
                    AuthorName = authorName,
                    PublisherName = publisherName
                }
            };

            _bookServicesMock.Setup(services => services.SearchBookDataWithDetail(bookTitle, authorName, publisherName))
                .ReturnsAsync(expectedResult);

            // Act 
            var result = await _booksController.SearchBookByCriteria(bookTitle, authorName, publisherName);

            // Assert
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
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertBookAsync()
        {
            // Arrange - valid book
            var validBookDto = new CreateBookDto
            {
                BookTitle = "NewBook",
                Quantity = "6",
                Price = "100",
                AuthorId = 1,
                PublisherId = 1
            };

            _numbersValidatorMock.Setup(services => services.IsValidInt(validBookDto.Quantity.ToString()))
                .Returns(true)
                .Verifiable();

            _numbersValidatorMock.Setup(services => services.IsValidDecimal(validBookDto.Price.ToString()))
                .Returns(true)
                .Verifiable();


            _mapperMock.Setup(m => m.Map<CreateBookDto, Book>(It.IsAny<CreateBookDto>()))
                .Returns((CreateBookDto dto) => new Book
                {
                    BookTitle = dto.BookTitle,
                    Quantity = int.Parse(dto.Quantity),
                    Price = decimal.Parse(dto.Price),
                    AuthorId = dto.AuthorId,
                    PublisherId = dto.PublisherId
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>().InsertAsync(It.IsAny<Book>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid book
            var successResult = await _booksController.InsertBookAsync(validBookDto);

            var okResult = successResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;
            Assert.NotNull(successResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.INSERTED, apiResponseOkResult.Message);



            // Arrange - Invalid quantity
            var InValidQuantityBookDto = new CreateBookDto
            {
                BookTitle = "NewBook",
                Quantity = "two",
                Price = "100",
                AuthorId = 1,
                PublisherId = 1
            };

            _numbersValidatorMock.Setup(services => services.IsValidInt(validBookDto.Quantity.ToString()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid quantity
            var errorResultInvalidQuantity = await _booksController.InsertBookAsync(InValidQuantityBookDto);

            // Assert - Invalid quantity
            var invalidQuantityBadRequestResult = errorResultInvalidQuantity as BadRequestObjectResult;
            var invalidQuantityApiResponse = invalidQuantityBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidQuantity);
            Assert.NotNull(invalidQuantityBadRequestResult);
            Assert.NotNull(invalidQuantityApiResponse);
            Assert.Equal(400, invalidQuantityBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_QUANTITY, invalidQuantityApiResponse.Message);



            // Arrange - Invalid price
            var InValidPriceBookDto = new CreateBookDto
            {
                BookTitle = "NewBook",
                Quantity = "6",
                Price = "hundred",
                AuthorId = 1,
                PublisherId = 1
            };

            _numbersValidatorMock.Setup(services => services.IsValidDecimal(validBookDto.Price.ToString()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid price
            var errorResultInvalidPrice = await _booksController.InsertBookAsync(InValidPriceBookDto);

            // Assert - Invalid price
            var invalidPriceBadRequestResult = errorResultInvalidPrice as BadRequestObjectResult;
            var invalidPriceApiResponse = invalidPriceBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidPrice);
            Assert.NotNull(invalidPriceBadRequestResult);
            Assert.NotNull(invalidPriceApiResponse);
            Assert.Equal(400, invalidPriceBadRequestResult.StatusCode);
        }
        #endregion

        #region Update
        [Fact]
        public async Task UpdateBook()
        {
            // Arrange - Valid book
            var validBookDto = new UpdateBookDto
            {
                Id = 1,
                BookTitle = "Updated Book",
                Quantity = "10",
                Price = "500",
                AuthorId = 1,
                PublisherId = 1
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Book>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _numbersValidatorMock.Setup(services => services.IsValidInt(validBookDto.Quantity.ToString()))
                .Returns(true)
                .Verifiable();

            _numbersValidatorMock.Setup(services => services.IsValidDecimal(validBookDto.Price.ToString()))
                .Returns(true)
                .Verifiable();

            _mapperMock.Setup(m => m.Map<UpdateBookDto, Book>(It.IsAny<UpdateBookDto>()))
                .Returns((UpdateBookDto dto) => new Book
                {
                    Id = dto.Id,
                    BookTitle = dto.BookTitle,
                    Quantity = int.Parse(dto.Quantity),
                    Price = int.Parse(dto.Price),
                    AuthorId = dto.AuthorId,
                    PublisherId = dto.PublisherId
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Book>().UpdateAsync(It.IsAny<Book>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid book
            var validResult = await _booksController.UpdateBookAsync(validBookDto);

            // Assert - Valid book
            var okResult = validResult as OkObjectResult;

            Assert.NotNull(validResult);
            Assert.NotNull(okResult);
            Assert.Equal(AppMessages.UPDATED, okResult.Value);


            // Arrange - Invalid book Id
            var InvalidIdBookDto = new UpdateBookDto
            {
                Id = 500,
                BookTitle = "Updated Book",
                Quantity = "10",
                Price = "500",
                AuthorId = 1,
                PublisherId = 1
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Book>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .Verifiable();

            // Act - Invalid book Id
            var invalidIdResult = await _booksController.UpdateBookAsync(InvalidIdBookDto);

            // Assert - Invalid book Id
            var invalidIdNotFoundResult = invalidIdResult as NotFoundObjectResult;
            var invalidIdApiResponse = invalidIdNotFoundResult.Value as ApiResponse;

            Assert.NotNull(invalidIdResult);
            Assert.NotNull(invalidIdNotFoundResult);
            Assert.NotNull(invalidIdApiResponse);
            Assert.Equal(404, invalidIdApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidIdApiResponse.Message);


            // Arrange - Invalid quantity
            var InValidQuantityEmployeeDto = new UpdateBookDto
            {
                Id = 1,
                BookTitle = "Updated Book",
                Quantity = "ten",
                Price = "500",
                AuthorId = 1,
                PublisherId = 1
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Book>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _numbersValidatorMock.Setup(services => services.IsValidInt(InValidQuantityEmployeeDto.Quantity.ToString()))
               .Returns(false)
               .Verifiable();

            // Act - Invalid quantity
            var errorResultInvalidQuantity = await _booksController.UpdateBookAsync(InValidQuantityEmployeeDto);

            // Assert - Invalid quantity
            var invalidQuantityBadRequestResult = errorResultInvalidQuantity as BadRequestObjectResult;
            var invalidQuantityApiResponse = invalidQuantityBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidQuantity);
            Assert.NotNull(invalidQuantityBadRequestResult);
            Assert.NotNull(invalidQuantityApiResponse);
            Assert.Equal(400, invalidQuantityBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_QUANTITY, invalidQuantityApiResponse.Message);


            // Arrange - Invalid price
            var InValidPriceEmployeeDto = new UpdateBookDto
            {
                Id = 1,
                BookTitle = "Updated Book",
                Quantity = "10",
                Price = "hundred",
                AuthorId = 1,
                PublisherId = 1
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Book>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _numbersValidatorMock.Setup(services => services.IsValidDecimal(InValidPriceEmployeeDto.Price.ToString()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid price
            var errorResultInvalidPrice = await _booksController.UpdateBookAsync(InValidPriceEmployeeDto);

            // Assert - Invalid price
            var invalidPriceBadRequestResult = errorResultInvalidPrice as BadRequestObjectResult;
            var invalidPriceApiResponse = invalidPriceBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidPrice);
            Assert.NotNull(invalidPriceBadRequestResult);
            Assert.NotNull(invalidPriceApiResponse);
            Assert.Equal(400, invalidPriceBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_PRICE, invalidPriceApiResponse.Message);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteBookAsync()
        {
            // Arrange
            int bookId = 1;
            var bookOrderDetailsRepositoryMock = new Mock<IGenericRepository<BookOrderDetails>>();

            var bookOrderDetails = new List<BookOrderDetails>
            {
                new BookOrderDetails { Id = 1, OrderId = 1, BookId = bookId , Price = 500, Quantity = 1}
            };

            var book = new Book
            {
                Id = bookId,
                BookTitle = "BookOne",
                Quantity = 10,
                Price = 500,
                AuthorId = 1,
                PublisherId = 1
            };

            var bookOrderDetailsSpec = new BookOrderDetailsWithBookAndCustomerSpec(null, bookId);
            bookOrderDetailsRepositoryMock.Setup(repo => repo.FindAllSpec(bookOrderDetailsSpec))
                .ReturnsAsync(bookOrderDetails)
                .Verifiable();

            _bookRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()))
                .ReturnsAsync(book)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<BookOrderDetails>())
                .Returns(bookOrderDetailsRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Book>())
                .Returns(_bookRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _booksController.DeleteBookAsync(bookId);

            //Assert 
            if (bookOrderDetails.Count > 0)
            {
                var okResult = result as OkObjectResult;
                var apiResponseOkResult = okResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(okResult);
                Assert.NotNull(apiResponseOkResult);
                Assert.Equal(201, apiResponseOkResult.StatusCode);
                Assert.Equal(AppMessages.DELETED, apiResponseOkResult.Message);

                bookOrderDetailsRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BookOrderDetailsWithBookAndCustomerSpec>()), Times.Once);
                _bookRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()), Times.Once);
                _bookRepositoryMock.Verify(repo => repo.DeleteAsync(book), Times.Once);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadRequest = badRequestResult.Value as ApiResponse;

                Assert.Equal(400, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.FAILED_DELETE, apiResponseBadRequest.Message);

                bookOrderDetailsRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BookOrderDetailsWithBookAndCustomerSpec>()), Times.Once);
                _bookRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<BooksWithAuthorAndPublisherSpec>()), Times.Never);
                _bookRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Book>()), Times.Never);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
            }
        }
        #endregion

        #region Data
        private List<Book> GetBooksData()
        {
            List<Book> booksData = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    BookTitle = "BookOne",
                    Quantity = 10,
                    Price = 500,
                    AuthorId = 1,
                    PublisherId = 1
                },

                new Book
                {
                    Id = 2,
                    BookTitle = "BookTwo",
                    Quantity = 8,
                    Price = 200,
                    AuthorId = 1,
                    PublisherId = 1
                },

                new Book
                {
                    Id = 3,
                    BookTitle = "BookThree",
                    Quantity = 7,
                    Price = 300,
                    AuthorId = 1,
                    PublisherId = 1
                }
            };
            return booksData;
        }

        private IReadOnlyList<ReadBookDto> GetReadBooksDtoData()
        {
            List<ReadBookDto> booksDtoData = new List<ReadBookDto>
            {
                new ReadBookDto
                {
                    Id = 1,
                    BookTitle = "BookOne",
                    Quantity = 10,
                    Price = 500,
                    AuthorName = "AuthorOne",
                    PublisherName = "PublisherOne"
                },

                new ReadBookDto
                {
                    Id = 2,
                    BookTitle = "BookTwo",
                    Quantity = 8,
                    Price = 200,
                    AuthorName = "AuthorTwo",
                    PublisherName = "PublisherTwo"
                },

                new ReadBookDto
                {
                    Id = 3,
                    BookTitle = "BookThree",
                    Quantity = 7,
                    Price = 300,
                    AuthorName = "AuthorThree",
                    PublisherName = "PublisherThree"
                }
            };

            return booksDtoData;
        }

        #endregion
    }
}
