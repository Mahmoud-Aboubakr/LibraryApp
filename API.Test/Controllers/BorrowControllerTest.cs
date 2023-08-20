using API.Controllers;
using Application.DTOs.Borrow;
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
using static System.Reflection.Metadata.BlobBuilder;
using Infrastructure.Specifications.BorrowSpec;
using Persistence.Repositories;
using Application.DTOs.Book;
using Microsoft.AspNetCore.Http;

namespace API.Test
{
    public class BorrowControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BorrowsController>> _loggerMock;
        private readonly BorrowsController _borrowsController;
        private readonly Mock<INumbersValidator> _numberValidatorMock;
        private readonly Mock<IBorrowServices> _borrowServicesMock;
        private readonly Mock<IGenericRepository<Borrow>> _borrowRepositoryMock;

        public BorrowControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _numberValidatorMock = new Mock<INumbersValidator>();
            _borrowServicesMock = new Mock<IBorrowServices>();
            _loggerMock = new Mock<ILogger<BorrowsController>>();
            _borrowRepositoryMock = new Mock<IGenericRepository<Borrow>>();
            _borrowsController = new BorrowsController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _borrowServicesMock.Object,
                                                        _numberValidatorMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        [Fact]
        public async Task GetAllBorrows()
        {
            // Arrange
            var pageSize = 6;
            var pageIndex = 1;
            var isPagingEnabled = true;


            var borrows = GetBorrowData();

            var readBorrowsDtos = GetReadBorrowDtoData();

            _borrowRepositoryMock.Setup(repo => repo.FindAllSpec(It.IsAny<BorrowWithBookAndCustomerSpec>()))
               .ReturnsAsync(borrows);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Borrow>())
                    .Returns(_borrowRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadBorrowDto>>(borrows)).Returns(readBorrowsDtos).Verifiable();
            // Act
            var result = await _borrowsController.GetAllBorrowsWithDetails(pageSize, pageIndex, isPagingEnabled);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Pagination<ReadBorrowDto>>>(result);
        }

        #endregion

        #region GetById
        [Fact]
        public async Task GetBorrowById()
        {
            //Arrange
            var borrows = GetBorrowData();
            int valid_borrowId = 1;
            int invalid_borrowId = 200;
            var validBorrow = borrows[0];

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Borrow>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => borrows.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Borrow>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => borrows.Find(a => a.Id == id))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Borrow, ReadBorrowDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Borrow, ReadBorrowDto>(It.IsAny<Borrow>()))
                .Returns((Borrow borrow) => mapper.Map<ReadBorrowDto>(borrow))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _borrowsController.GetById(valid_borrowId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedBorrow = validOkResult.Value as ReadBorrowDto;

            // Act - Invalid ID
            var invalidActionResult = await _borrowsController.GetById(invalid_borrowId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedBorrow);
            Assert.Equal(validBorrow.Id, validReturnedBorrow.Id);
            Assert.Equal(validBorrow.CustomerId, validReturnedBorrow.CustomerId);
            Assert.Equal(validBorrow.BookId, validReturnedBorrow.BookId);
            Assert.Equal(validBorrow.BorrowDate, validReturnedBorrow.BorrowDate);
            Assert.Equal(validBorrow.ReturnDate, validReturnedBorrow.ReturnDate);

            // Assert - Invalid ID
            Assert.NotNull(invalidNotFoundResult);
            Assert.NotNull(invalidApiResponse);
            Assert.Equal(404, invalidNotFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidApiResponse.Message);
        }
        #endregion

        #region GetByIdWithIncludes
        [Fact]
        public async Task GetByIdWithIncludesAsync_Ok_WhenBorrowExists()
        {
            // Arrange
            var id = "1";
            var borrow = new Borrow
            {
                CustomerId = 1,
                BookId = 1
            };
            var borrowDto = new ReadBorrowDto
            {
                CustomerId = 1,
                BookId = 1
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>().Exists(int.Parse(id))).ReturnsAsync(true);
            var spec = new BorrowWithBookAndCustomerSpec(int.Parse(id));
            _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>().FindSpec(spec)).ReturnsAsync(borrow);
            _mapperMock.Setup(m => m.Map<Borrow, ReadBorrowDto>(borrow)).Returns(borrowDto);

            // Act
            var result = await _borrowsController.GetByIdWithIncludesAsync(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var successResult = result as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(successResult);
        }

        [Fact]
        public async Task GetByIdWithIncludesAsync_NotFound_WhenIdIsInvalid()
        {
            // Arrange
            var id = "-1";
            var borrow = new Borrow
            {
                CustomerId = 1,
                BookId = 1
            };
            var borrowDto = new ReadBorrowDto
            {
                CustomerId = 1,
                BookId = 1
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>().Exists(int.Parse(id))).ReturnsAsync(false);

            // Act
            var result = await _borrowsController.GetByIdWithIncludesAsync(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, (notFoundResult.Value as ApiResponse).Message);
        }
      
        #endregion

        #region Search
        [Fact]
        public async Task SearchBorrowWithCriteria()
        {
            // Arrange 
            string validCustomerName = "Rita";
            string validBookTitle = "DataBase";
            DateTime validDate = DateTime.Now;

            var expectedResult = GetReadBorrowDtoData();

            _borrowServicesMock.Setup(services => services.SearchWithCriteria(validCustomerName, validBookTitle, validDate))
                .ReturnsAsync(expectedResult);

            // Act 
            var result = await _borrowsController.SearchByCriteria(validCustomerName, validBookTitle, validDate);

            // Assert 
            if (result == null)
            {
                var errorResult = result.Result as NotFoundObjectResult;
                var apiResponseBadRequest = errorResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(errorResult);
                Assert.NotNull(apiResponseBadRequest);
                Assert.Equal(404, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.NOTFOUND_SEARCHDATA, apiResponseBadRequest.Message);
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
        public async Task InsertBorrowAsync_ShouldReturnCreated_WhenBorrowIsValid()
        {
            // Arrange
            var borrowDto = new CreateBorrowDto 
            {
                CustomerId = "1",
                BookId = "2"
            };
            var borrow = new Borrow { CustomerId = 1, BookId = 2 };
            _borrowServicesMock.Setup(s => s.IsBannedCustomer(borrowDto.CustomerId)).ReturnsAsync(false);
            _numberValidatorMock.Setup(v => v.IsValidInt(borrowDto.CustomerId)).Returns(true);
            _numberValidatorMock.Setup(v => v.IsValidInt(borrowDto.BookId)).Returns(true);
            _borrowServicesMock.Setup(s => s.CreateBorrowValidator(borrowDto.CustomerId)).Returns(true);
            _mapperMock.Setup(m => m.Map<CreateBorrowDto, Borrow>(borrowDto)).Returns(borrow);
            _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(_borrowRepositoryMock.Object);

            // Act
            var result = await _borrowsController.InsertBorrowAsync(borrowDto);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task InsertBorrowAsync_ShouldReturnBadRequest_WhenCustomerIsBanned()
        {
            // Arrange
            var borrowDto = new CreateBorrowDto
            { 
                CustomerId = "8",
                BookId = "2" };
            _borrowServicesMock.Setup(s => s.IsBannedCustomer(borrowDto.CustomerId)).ReturnsAsync(true);

            // Act
            var result = await _borrowsController.InsertBorrowAsync(borrowDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(AppMessages.BANNED_CUSTOMER, (badRequestResult.Value as ApiResponse).Message);
        }

        [Fact]
        public async Task InsertBorrowAsync_ShouldReturnBadRequest_WhenCustomerHasMaxBorrows()
        {
            // Arrange
            var borrowDto = new CreateBorrowDto { CustomerId = "1", BookId = "2" };
            _borrowServicesMock.Setup(s => s.IsBannedCustomer(borrowDto.CustomerId)).ReturnsAsync(false);
            _numberValidatorMock.Setup(v => v.IsValidInt(borrowDto.CustomerId)).Returns(true);
            _numberValidatorMock.Setup(v => v.IsValidInt(borrowDto.BookId)).Returns(true);
            _borrowServicesMock.Setup(s => s.CreateBorrowValidator(borrowDto.CustomerId)).Returns(false);

            // Act
            var result = await _borrowsController.InsertBorrowAsync(borrowDto);
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(AppMessages.MAX_BORROWING, (badRequestResult.Value as ApiResponse).Message);
        }
        #endregion

        #region Delete

        [Fact]
        public async Task DeleteBorrowAsync()
        {
            // Arrange
            var borrowId = 1;
            var borrow = new Borrow
            {
                Id = 1,
                CustomerId = 1,
                BookId = 4,
                BorrowDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(3)
            };

            _borrowRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<BorrowWithBookAndCustomerSpec>()))
                     .ReturnsAsync(borrow)
                     .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Borrow>())
                .Returns(_borrowRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _borrowsController.DeleteBorrowAsync(borrowId);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<string>(okResult.Value);
            var message = okResult.Value as string;
            Assert.Equal(AppMessages.DELETED, message);
        }

        #endregion

        #region Data
        private List<Borrow> GetBorrowData()
        {
            List<Borrow> borrowsData = new List<Borrow>
            {
                new Borrow
                {
                    Id = 1,
                    CustomerId = 1,
                    BookId= 4,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },

                new Borrow
                {
                   Id = 2,
                    CustomerId = 2,
                    BookId= 3,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },

                new Borrow
                {
                    Id = 3,
                    CustomerId = 3,
                    BookId= 9,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },
            };

            return borrowsData;
        }

        private List<ReadBorrowDto> GetReadBorrowDtoData()
        {
            List<ReadBorrowDto> readBorrowsDtoData = new List<ReadBorrowDto>
            {
                new ReadBorrowDto
                {
                    Id = 1,
                    CustomerId = 1,
                    CustomerName="Rita",
                    BookId= 4,
                    BookName="DataBase",
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },

                new ReadBorrowDto
                {
                    Id = 2,
                    CustomerId = 2,
                    BookId= 3,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },

                new ReadBorrowDto
                {
                    Id = 3,
                    CustomerId = 3,
                    BookId= 9,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3)
                },
            };

            return readBorrowsDtoData;
        }
        #endregion
    }
}
