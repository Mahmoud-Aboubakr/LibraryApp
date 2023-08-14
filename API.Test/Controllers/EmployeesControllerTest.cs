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
using System.Net;
using Application.DTOs.Employee;
using Application.DTOs.Customer;

namespace API.Test
{
    public class EmployeesControllerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EmployeesController>> _loggerMock;
        private readonly EmployeesController _employeesController;
        private readonly Mock<IPhoneNumberValidator> _phoneNumberValidatorMock;
        private readonly Mock<IEmployeeServices> _employeeServicesMock;

        public EmployeesControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _phoneNumberValidatorMock = new Mock<IPhoneNumberValidator>();
            _employeeServicesMock = new Mock<IEmployeeServices>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
            _employeesController = new EmployeesController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _phoneNumberValidatorMock.Object,
                                                        _employeeServicesMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetById
        [Fact]
        public async Task GetAuthorById_ReturnsAuthor()
        {
            //Arrange
            var employees = GetEmployeesData();
            int valid_employeeId = 1;
            int invalid_employeeId = 200;
            var validEmployee = employees[0];
            var validReadEmployeeDto = new ReadEmployeeDto
            {
                Id = validEmployee.Id,
                EmpName = validEmployee.EmpName,
                EmpType = validEmployee.EmpType,
                EmpAge = validEmployee.EmpAge,
                EmpAddress = validEmployee.EmpAddress,
                EmpPhoneNumber = validEmployee.EmpPhoneNumber,
                EmpStartingShift = validEmployee.EmpStartingShift,
                EmpEndingShift = validEmployee.EmpEndingShift,
                EmpBasicSalary = validEmployee.EmpBasicSalary
            };

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>().Exists(It.IsAny<int>()))
                .ReturnsAsync((int id) => employees.Exists(a => a.Id == id))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>().GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => employees.Find(a => a.Id == id))
                .Verifiable();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, ReadEmployeeDto>();
            });
            var mapper = mapperConfig.CreateMapper();

            _mapperMock.Setup(m => m.Map<Employee, ReadEmployeeDto>(It.IsAny<Employee>()))
                .Returns((Employee employee) => mapper.Map<ReadEmployeeDto>(employee))
                .Verifiable();

            // Act - Valid ID
            var validActionResult = await _employeesController.GetEmployeeByIdAsync(valid_employeeId.ToString());
            var validOkResult = validActionResult as OkObjectResult;
            var validReturnedEmployee = validOkResult.Value as ReadEmployeeDto;

            // Act - Invalid ID
            var invalidActionResult = await _employeesController.GetEmployeeByIdAsync(invalid_employeeId.ToString());
            var invalidNotFoundResult = invalidActionResult as NotFoundObjectResult;
            var invalidApiResponse = invalidNotFoundResult.Value as ApiResponse;

            // Assert - Valid ID
            Assert.NotNull(validActionResult);
            Assert.NotNull(validOkResult);
            Assert.NotNull(validReturnedEmployee);
            Assert.Equal(validEmployee.Id, validReturnedEmployee.Id);
            Assert.Equal(validEmployee.EmpName, validReturnedEmployee.EmpName);
            Assert.Equal(validEmployee.EmpType, validReturnedEmployee.EmpType);
            Assert.Equal(validEmployee.EmpAge, validReturnedEmployee.EmpAge);
            Assert.Equal(validEmployee.EmpAddress, validReturnedEmployee.EmpAddress);
            Assert.Equal(validEmployee.EmpPhoneNumber, validReturnedEmployee.EmpPhoneNumber);
            Assert.Equal(validEmployee.EmpStartingShift, validReturnedEmployee.EmpStartingShift);
            Assert.Equal(validEmployee.EmpEndingShift, validReturnedEmployee.EmpEndingShift);
            Assert.Equal(validEmployee.EmpBasicSalary, validReturnedEmployee.EmpBasicSalary);

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
        public async Task SearchEmployeeWithCriteria()
        {
            // Arrange - Valid employee
            string validName = "Rawan";
            byte validType = 3;
            string validPhone = "01013451622";
            decimal validSalary = 8000;

            var expectedResult = new List<ReadEmployeeDto>
            {
                new ReadEmployeeDto
                {
                    Id = 1,
                    EmpName = validName,
                    EmpType = validType,
                    EmpAge = 21,
                    EmpAddress = "Cairo",
                    EmpPhoneNumber = validPhone,
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = validSalary
                }
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(validType))
                .Returns(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.SearchEmployeeDataWithDetail(validName, validType, validPhone, validSalary))
                .ReturnsAsync(expectedResult);

            // Act - Valid employee
            var result = await _employeesController.SearchEmployeeWithCriteria(validName, validType.ToString(), validPhone, validSalary.ToString());

            // Assert - Valid employee
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
            var ErrorResultInvalidPhoneNumber = await _employeesController.SearchEmployeeWithCriteria(validName, validType.ToString(), InvalidPhone, validSalary.ToString());

            // Assert - Invalid phone number
            var invalidPhoneNumberBadRequestResult = ErrorResultInvalidPhoneNumber.Result as BadRequestObjectResult;
            var invalidPhoneNumberApiResponse = invalidPhoneNumberBadRequestResult.Value as ApiResponse;

            Assert.NotNull(ErrorResultInvalidPhoneNumber);
            Assert.NotNull(invalidPhoneNumberBadRequestResult);
            Assert.NotNull(invalidPhoneNumberApiResponse);
            Assert.Equal(400, invalidPhoneNumberApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidPhoneNumberApiResponse.Message);


            // Arrange - Invalid type
            byte InvalidType = 5;

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(InvalidType))
                .Returns(false)
                .Verifiable();

            // Act - Invalid type
            var ErrorResultInvalidType = await _employeesController.SearchEmployeeWithCriteria(validName, InvalidType.ToString(), validPhone, validSalary.ToString());

            // Assert - Invalid type
            var invalidTypeBadRequestResult = ErrorResultInvalidType.Result as BadRequestObjectResult;
            var invalidTypeApiResponse = invalidTypeBadRequestResult.Value as ApiResponse;

            Assert.NotNull(ErrorResultInvalidType);
            Assert.NotNull(invalidTypeBadRequestResult);
            Assert.NotNull(invalidTypeApiResponse);
            Assert.Equal(400, invalidTypeApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidTypeApiResponse.Message);
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertAuthorAsync()
        {
            // Arrange - valid employee
            var validEmployeeDto = new CreateEmployeeDto
            {
                EmpName = "Ahmed",
                EmpType = 1,
                EmpAge = 24,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01286892250",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 6000
            };

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(validEmployeeDto.EmpType))
                .Returns(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeAge(validEmployeeDto.EmpAge))
                .Returns(true)
                .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

            _mapperMock.Setup(m => m.Map<CreateEmployeeDto, Employee>(It.IsAny<CreateEmployeeDto>()))
                .Returns((CreateEmployeeDto dto) => new Employee
                {
                    EmpName = dto.EmpName,
                    EmpType = dto.EmpType,
                    EmpAge = dto.EmpAge,
                    EmpAddress = dto.EmpAddress,
                    EmpPhoneNumber = dto.EmpPhoneNumber,
                    EmpStartingShift = dto.EmpStartingShift,
                    EmpEndingShift = dto.EmpEndingShift,
                    EmpBasicSalary = dto.EmpBasicSalary
                })
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>().InsertAsync(It.IsAny<Employee>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid employee
            var successResult = await _employeesController.InsertEmployeeAsync(validEmployeeDto);

            var okResult = successResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;
            Assert.NotNull(successResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.INSERTED, apiResponseOkResult.Message);



            // Arrange - Invalid type
            var InValidTypeEmployeeDto = new CreateEmployeeDto
            {
                EmpName = "Ahmed",
                EmpType = 5,
                EmpAge = 24,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01286892250",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 6000
            };

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(InValidTypeEmployeeDto.EmpType))
                .Returns(false)
                .Verifiable();

            // Act - Invalid type
            var errorResultInvalidType = await _employeesController.InsertEmployeeAsync(InValidTypeEmployeeDto);

            // Assert - Invalid type
            var invalidTypeBadRequestResult = errorResultInvalidType as BadRequestObjectResult;
            var invalidTypeApiResponse = invalidTypeBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidType);
            Assert.NotNull(invalidTypeBadRequestResult);
            Assert.NotNull(invalidTypeApiResponse);
            Assert.Equal(400, invalidTypeBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_EMPTYPE, invalidTypeApiResponse.Message);



            // Arrange - Invalid age
            var InValidAgeEmployeeDto = new CreateEmployeeDto
            {
                EmpName = "Ahmed",
                EmpType = 1,
                EmpAge = 0,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01286892250",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 6000
            };

            _employeeServicesMock.Setup(services => services.IsValidEmployeeAge(InValidAgeEmployeeDto.EmpAge))
                .Returns(false)
                .Verifiable();

            // Act - Invalid age
            var errorResultInvalidAge = await _employeesController.InsertEmployeeAsync(InValidAgeEmployeeDto);

            // Assert - Invalid age
            var invalidAgeBadRequestResult = errorResultInvalidAge as BadRequestObjectResult;
            var invalidAgeApiResponse = invalidAgeBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidAge);
            Assert.NotNull(invalidAgeBadRequestResult);
            Assert.NotNull(invalidAgeApiResponse);
            Assert.Equal(400, invalidAgeBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_AGE, invalidAgeApiResponse.Message);



            // Arrange - Invalid phone number
            var InValidPhoneEmployeeDto = new CreateEmployeeDto
            {
                EmpName = "Ahmed",
                EmpType = 1,
                EmpAge = 24,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "011622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 6000
            };

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var errorResultInvalidPhoneNumber = await _employeesController.InsertEmployeeAsync(InValidPhoneEmployeeDto);

            // Assert - Invalid phone number
            var invalidPhoneNumberBadRequestResult = errorResultInvalidPhoneNumber as BadRequestObjectResult;
            var invalidPhoneNumberApiResponse = invalidPhoneNumberBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidPhoneNumber);
            Assert.NotNull(invalidPhoneNumberBadRequestResult);
            Assert.NotNull(invalidPhoneNumberApiResponse);
            Assert.Equal(400, invalidPhoneNumberBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidPhoneNumberApiResponse.Message);


            // Verify the interactions with the mocks
            _phoneNumberValidatorMock.Verify();
            _mapperMock.Verify();
            _unitOfWorkMock.Verify();
        }
        #endregion



        #region Data
        private List<Employee> GetEmployeesData()
        {
            List<Employee> employeesData = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    EmpName = "Rawan",
                    EmpType = 3,
                    EmpAge = 21,
                    EmpAddress = "Cairo",
                    EmpPhoneNumber = "01013451622",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 8000
                },

                new Employee
                {
                    Id = 2,
                    EmpName = "Rita",
                    EmpType = 0,
                    EmpAge = 21,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01013887622",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 6000
                },

                new Employee
                {
                    Id = 3,
                    EmpName = "Ali",
                    EmpType = 1,
                    EmpAge = 25,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01013887699",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 4000
                },

                new Employee
                {
                    Id = 4,
                    EmpName = "Omar",
                    EmpType = 2,
                    EmpAge = 27,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01223887699",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 5000
                }
            };

            return employeesData;
        }

        private List<ReadEmployeeDto> GetReadEmployeesDtoData()
        {
            List<ReadEmployeeDto> employeesDtoData = new List<ReadEmployeeDto>
            {
                new ReadEmployeeDto
                {
                    Id = 1,
                    EmpName = "Rawan",
                    EmpType = 3,
                    EmpAge = 21,
                    EmpAddress = "Cairo",
                    EmpPhoneNumber = "01013451622",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 8000
                },

                new ReadEmployeeDto
                {
                    Id = 2,
                    EmpName = "Rita",
                    EmpType = 0,
                    EmpAge = 21,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01013887622",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 6000
                },

                new ReadEmployeeDto
                {
                    Id = 3,
                    EmpName = "Ali",
                    EmpType = 1,
                    EmpAge = 25,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01013887699",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 4000
                },

                new ReadEmployeeDto
                {
                    Id = 4,
                    EmpName = "Omar",
                    EmpType = 2,
                    EmpAge = 27,
                    EmpAddress = "Alex",
                    EmpPhoneNumber = "01223887699",
                    EmpStartingShift = DateTime.Now,
                    EmpEndingShift = DateTime.Now.AddHours(8),
                    EmpBasicSalary = 5000
                }
            };

            return employeesDtoData;
        }

        #endregion
    }
}
