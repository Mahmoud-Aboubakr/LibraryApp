using API.Controllers;
using Application.Interfaces.IAppServices;
using Application.Interfaces.IValidators;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs.Author;
using Application.Handlers;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Customer;
using Infrastructure.Specifications.BookSpec;
using Infrastructure.Specifications.BannedCustomerSpec;
using Infrastructure.Specifications.CustomerSpec;
using Application;

namespace API.Test
{
    public class CustomersControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CustomersController>> _loggerMock;
        private readonly CustomersController _customersController;
        private readonly Mock<IPhoneNumberValidator> _phoneNumberValidatorMock;
        private readonly Mock<ICustomerServices> _customerServicesMock;

        public CustomersControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _phoneNumberValidatorMock = new Mock<IPhoneNumberValidator>();
            _customerServicesMock = new Mock<ICustomerServices>();
            _loggerMock = new Mock<ILogger<CustomersController>>();
            _customersController = new CustomersController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _phoneNumberValidatorMock.Object,
                                                        _customerServicesMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        //[Fact]
        //public async Task GetAllCustomers()
        //{
        //    // Arrange
        //    var pageSize = 6;
        //    var pageIndex = 1;
        //    var isPagingEnabled = true;
        //    var spec = new CustomerSpec(pageSize, pageIndex, isPagingEnabled);
        //    var totalCustomers = 6;

        //    var customers = GetCustomersData();

        //    var readCustomerDtos = GetReadCustomersDtoData();

        //    var paginationData = new Pagination<ReadCustomerDto>(pageIndex, pageSize, totalCustomers, readCustomerDtos);

        //    var customerRepositoryMock = new Mock<IGenericRepository<Customer>>();

        //    customerRepositoryMock.Setup(repo => repo.CountAsync(spec))
        //        .ReturnsAsync(totalCustomers)
        //        .Verifiable();

        //    customerRepositoryMock.Setup(repo => repo.FindAllSpec(spec))
        //        .ReturnsAsync(customers)
        //        .Verifiable();

        //    _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>())
        //            .Returns(customerRepositoryMock.Object)
        //            .Verifiable();

        //    _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadCustomerDto>>(customers)).Returns(readCustomerDtos).Verifiable();

        //    // Act
        //    var result = await _customersController.GetAllCustomerAsync(pageSize, pageIndex, isPagingEnabled);

        //    Assert.NotNull(result);
        //    Assert.IsType<ActionResult<Pagination<ReadAuthorDto>>>(result);
        //    //Assert.IsType<OkObjectResult>(result.Result);
        //    //var okResult = result as ActionResult<Pagination<ReadCustomerDto>>;
        //    //var okResult = result.Result as OkObjectResult;
        //    //var paginationResult = okResult.Value as Pagination<ReadCustomerDto>;
        //    //Assert.IsType<Pagination<ReadCustomerDto>>(okResult.Value);
        //    var paginationResult = result.Value as Pagination<ReadCustomerDto>;
        //    //Assert.Equal(paginationData, okResult);
        //    Assert.Equal(paginationData, paginationResult);

        //    customerRepositoryMock.Verify(repo => repo.CountAsync(spec), Times.Once);
        //    customerRepositoryMock.Verify(repo => repo.FindAllSpec(spec), Times.Once);
        //    _mapperMock.Verify(mapper => mapper.Map<IReadOnlyList<Author>>(customers), Times.Once);

        //    // Assert Null
        //    Assert.IsType<NotFoundObjectResult>(result.Result);
        //    var notFoundResult = result.Result as NotFoundObjectResult;
        //    Assert.IsType<ApiResponse>(notFoundResult.Value);
        //    var apiResponse = notFoundResult.Value as ApiResponse;
        //    Assert.Equal(404, apiResponse.StatusCode);

        //    _unitOfWorkMock.Verify();
        //    _mapperMock.Verify();
        //}

        //[Fact]
        //public async Task GetAllCustomers()
        //{
        //    // Arrange
        //    int pageSize = 6;
        //    int pageIndex = 1;
        //    bool isPagingEnabled = true;

        //    var customers = GetCustomersData();

        //    var readCustomerDtos = GetReadCustomersDtoData();

        //    var spec = new CustomerSpec(pageSize, pageIndex, isPagingEnabled);
        //    var totalCustomers = 6;

        //    var customerRepositoryMock = new Mock<IGenericRepository<Customer>>();

        //    customerRepositoryMock.Setup(repo => repo.CountAsync(spec))
        //        .ReturnsAsync(totalCustomers)
        //        .Verifiable();

        //    customerRepositoryMock.Setup(repo => repo.FindAllSpec(spec))
        //        .ReturnsAsync(customers)
        //        .Verifiable();

        //    _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>())
        //        .Returns(customerRepositoryMock.Object)
        //        .Verifiable();

        //    var mapperConfig = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Customer, ReadCustomerDto>();
        //    });
        //    var mapper = mapperConfig.CreateMapper();

        //    _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadCustomerDto>>(It.IsAny<Author>()))
        //        .Returns((Customer customer) => mapper.Map<IReadOnlyList<ReadCustomerDto>>(customer))
        //        .Verifiable();

        //    // Act
        //    var result = await _customersController.GetAllCustomerAsync(pageSize, pageIndex, isPagingEnabled);

        //    // Assert
        //    var paginationData = new Pagination<ReadCustomerDto>(pageIndex, pageSize, totalCustomers, readCustomerDtos);

        //    //Assert.NotNull(result);
        //    //Assert.IsType<ActionResult<Pagination<ReadCustomerDto>>>(result);
        //    //Assert.Equal(readCustomerDtos, result.Value);
        //    //Assert.Equal(paginationData, result.Value);
        //    Assert.NotNull(result);
        //    Assert.IsType<ActionResult<Pagination<ReadCustomerDto>>>(result);
        //    Assert.Equal(paginationData, result.Value);

        //    customerRepositoryMock.Verify(repo => repo.CountAsync(spec), Times.Once);
        //    customerRepositoryMock.Verify(repo => repo.FindAllSpec(spec), Times.Once);
        //    _mapperMock.Verify(mapper => mapper.Map<IReadOnlyList<Customer>>(customers), Times.Once);
        //}

        #endregion

        #region GetById
        [Fact]
        public async Task GetCustomerById()
        {
            //Arrange
            var customers = GetCustomersData();
            int valid_customerId = 1;
            int invalid_customerId = 200;
            var validCustomer = customers[0];
            var validReadCustomerDto = new ReadCustomerDto
            {
                Id = validCustomer.Id,
                CustomerName = validCustomer.CustomerName,
                CustomerPhoneNumber = validCustomer.CustomerPhoneNumber,
                CustomerAddress = validCustomer.CustomerAddress
            };

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => customers.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => customers.Find(a => a.Id == id))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, ReadCustomerDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Customer, ReadCustomerDto>(It.IsAny<Customer>()))
                .Returns((Customer customer) => mapper.Map<ReadCustomerDto>(customer))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _customersController.GetCustomerByIdAsync(valid_customerId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedCustomer = validOkResult.Value as ReadCustomerDto;

            // Act - Invalid ID
            var invalidActionResult = await _customersController.GetCustomerByIdAsync(invalid_customerId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedCustomer);
            Assert.Equal(validCustomer.Id, validReturnedCustomer.Id);
            Assert.Equal(validCustomer.CustomerName, validReturnedCustomer.CustomerName);
            Assert.Equal(validCustomer.CustomerPhoneNumber, validReturnedCustomer.CustomerPhoneNumber);
            Assert.Equal(validCustomer.CustomerAddress, validReturnedCustomer.CustomerAddress);
            
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

        #region Search
        [Fact]
        public async Task SearchCustomerWithCriteria()
        {
            // Arrange - Valid phone number
            string validCustomerName = "Rawan";
            string validPhoneNumber = "01013451622";

            var expectedResult = new List<ReadCustomerDto>
            {
                new ReadCustomerDto
                {
                    Id = 1,
                    CustomerName = validCustomerName,
                    CustomerPhoneNumber = validPhoneNumber,
                    CustomerAddress = "Cairo"
                }
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _customerServicesMock.Setup(services => services.SearchWithCriteria(validCustomerName, validPhoneNumber))
                .ReturnsAsync(expectedResult);

            // Act - Valid phone number
            var result = await _customersController.SearchCustomerWithCriteria(validCustomerName, validPhoneNumber);

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
            string InvalidPhoneNumber = "011622";

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var ErrorResult = await _customersController.SearchCustomerWithCriteria(validCustomerName, InvalidPhoneNumber);

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
        public async Task InsertCustomerAsync()
        {
            // Arrange
            var customerDto = new CreateCustomerDto
            {
                CustomerName = "Ahmed",
                CustomerPhoneNumber = "01013451699",
                CustomerAddress = "Cairo"
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

            _mapperMock.Setup(m => m.Map<CreateCustomerDto, Customer>(It.IsAny<CreateCustomerDto>()))
                .Returns((CreateCustomerDto dto) => new Customer
                {
                    CustomerName = dto.CustomerName,
                    CustomerPhoneNumber = dto.CustomerPhoneNumber,
                    CustomerAddress = dto.CustomerAddress
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>().InsertAsync(It.IsAny<Customer>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                        .Returns(Task.FromResult(1));

            // Act
            var result = await _customersController.InsertCustomerAsync(customerDto);

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
        public async Task UpdateCustomerAsync()
        {
            // Arrange - Valid Customer
            var ValidCustomerDto = new ReadCustomerDto
            {
                Id = 1,
                CustomerName = "Updated Author",
                CustomerPhoneNumber = "01286892250",
                CustomerAddress = "Cairo"
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Customer>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _mapperMock.Setup(m => m.Map<ReadCustomerDto, Customer>(It.IsAny<ReadCustomerDto>()))
                .Returns((ReadCustomerDto dto) => new Customer
                {
                    Id = dto.Id,
                    CustomerName = dto.CustomerName,
                    CustomerPhoneNumber = dto.CustomerPhoneNumber,
                    CustomerAddress = dto.CustomerAddress
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Customer>().UpdateAsync(It.IsAny<Customer>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid Customer
            var validResult = await _customersController.UpdateCustomerAsync(ValidCustomerDto);

            // Assert - Valid Customer
            var okResult = validResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(validResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.UPDATED, apiResponseOkResult.Message);


            // Arrange - Invalid customer Id
            var InvalidCustomerDto = new ReadCustomerDto
            {
                Id = 500,
                CustomerName = "Updated Author",
                CustomerPhoneNumber = "01286892250",
                CustomerAddress = "Cairo"
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Customer>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .Verifiable();

            // Act - Invalid Customer Id
            var invalidIdResult = await _customersController.UpdateCustomerAsync(InvalidCustomerDto);

            // Assert - Invalid Customer Id
            var invalidIdNotFoundResult = invalidIdResult as NotFoundObjectResult;
            var invalidIdApiResponse = invalidIdNotFoundResult.Value as ApiResponse;

            Assert.NotNull(invalidIdResult);
            Assert.NotNull(invalidIdNotFoundResult);
            Assert.NotNull(invalidIdApiResponse);
            Assert.Equal(404, invalidIdApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidIdApiResponse.Message);


            // Arrange - Invalid phone number
            var invalidPhoneNumberCustomerDto = new ReadCustomerDto
            {
                Id = 1,
                CustomerName = "Updated Author",
                CustomerPhoneNumber = "0122250",
                CustomerAddress = "Cairo"
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Customer>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(true)
                    .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var invalidPhoneNumberResult = await _customersController.UpdateCustomerAsync(invalidPhoneNumberCustomerDto);

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
        public async Task DeleteCustomerAsync()
        {
            // Arrange
            int customerId = 1;
            var bannedCustomerRepositoryMock = new Mock<IGenericRepository<BannedCustomer>>();
            var customerRepositoryMock = new Mock<IGenericRepository<Customer>>();

            var bannedCustomers = new List<BannedCustomer>
            {
                new BannedCustomer { Id = 1, CustomerId = customerId,BanDate = DateTime.Now }
            };

            var customer = new Customer
            {
                Id = customerId,
                CustomerName = "Rawan",
                CustomerPhoneNumber = "01013451622",
                CustomerAddress = "Cairo"
            };

            var bannedCustomerSpec = new BannedCustomerWithEmployeeAndCustomerSpec(null, customerId);
            bannedCustomerRepositoryMock.Setup(repo => repo.FindAllSpec(bannedCustomerSpec))
                .ReturnsAsync(bannedCustomers)
                .Verifiable();

            customerRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<CustomerSpec>()))
                .ReturnsAsync(customer)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<BannedCustomer>())
                .Returns(bannedCustomerRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Customer>())
                .Returns(customerRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _customersController.DeleteCustomerAsync(customerId.ToString());

            //Assert 
            if (bannedCustomers.Count > 0)
            {
                var okResult = result as OkObjectResult;
                var apiResponseOkResult = okResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(okResult);
                Assert.NotNull(apiResponseOkResult);
                Assert.Equal(201, apiResponseOkResult.StatusCode);
                Assert.Equal(AppMessages.DELETED, apiResponseOkResult.Message);

                bannedCustomerRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BannedCustomerWithEmployeeAndCustomerSpec>()), Times.Once);
                customerRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<CustomerSpec>()), Times.Once);
                customerRepositoryMock.Verify(repo => repo.DeleteAsync(customer), Times.Once);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadRequest = badRequestResult.Value as ApiResponse;

                Assert.Equal(400, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.FAILED_DELETE, apiResponseBadRequest.Message);

                bannedCustomerRepositoryMock.Verify(repo => repo.FindAllSpec(It.IsAny<BannedCustomerWithEmployeeAndCustomerSpec>()), Times.Once);
                customerRepositoryMock.Verify(repo => repo.FindSpec(It.IsAny<CustomerSpec>()), Times.Never);
                customerRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Customer>()), Times.Never);
                _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
            }
        }
        #endregion


        #region Data
        private List<Customer> GetCustomersData()
        {
            List<Customer> customersData = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    CustomerName = "Rawan",
                    CustomerPhoneNumber = "01013451622",
                    CustomerAddress = "Cairo"
                },

                new Customer
                {
                    Id = 2,
                    CustomerName = "Rita",
                    CustomerPhoneNumber = "01013451644",
                    CustomerAddress = "Alex"
                },

                new Customer
                {
                    Id = 3,
                    CustomerName = "Ali",
                    CustomerPhoneNumber = "01055451699",
                    CustomerAddress = "Alex"
                }
            };

            return customersData;
        }

        private List<ReadCustomerDto> GetReadCustomersDtoData()
        {
            List<ReadCustomerDto> customersDtoData = new List<ReadCustomerDto>
            {
                new ReadCustomerDto
                {
                    Id = 1,
                    CustomerName = "Rawan",
                    CustomerPhoneNumber = "01013451622",
                    CustomerAddress = "Cairo"
                },

                new ReadCustomerDto
                {
                    Id = 2,
                    CustomerName = "Rita",
                    CustomerPhoneNumber = "01013451644",
                    CustomerAddress = "Alex"
                },

                new ReadCustomerDto
                {
                    Id = 3,
                    CustomerName = "Ali",
                    CustomerPhoneNumber = "01055451699",
                    CustomerAddress = "Alex"
                }
            };

            return customersDtoData;
        }

        #endregion

    }
}
