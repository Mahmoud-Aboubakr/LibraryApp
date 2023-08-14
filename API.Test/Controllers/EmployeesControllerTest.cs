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
using Application;
using Infrastructure.Specifications.EmployeeSpec;
using Infrastructure.Specifications.BookSpec;
using System.Security;
using Microsoft.VisualBasic;
using Infrastructure.Specifications.AttendanceSpec;
using static System.Reflection.Metadata.BlobBuilder;
using Infrastructure.Specifications.PayrollSpec;
using Infrastructure.Specifications.VacationSpec;

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
        private readonly Mock<IGenericRepository<Employee>> _employeeRepositoryMock;

        public EmployeesControllerTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _phoneNumberValidatorMock = new Mock<IPhoneNumberValidator>();
            _employeeServicesMock = new Mock<IEmployeeServices>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
            _employeeRepositoryMock = new Mock<IGenericRepository<Employee>>();
            _employeesController = new EmployeesController(_unitOfWorkMock.Object,
                                                        _mapperMock.Object,
                                                        _phoneNumberValidatorMock.Object,
                                                        _employeeServicesMock.Object,
                                                        _loggerMock.Object);
        }

        #region GetAll
        [Fact]
        public async Task GetAllEmployees()
        {
            // Arrange
            var pageSize = 6;
            var pageIndex = 1;
            var isPagingEnabled = true;


            var employees = GetEmployeesData();

            var readEmployeeDtos = GetReadEmployeesDtoData();

            _employeeRepositoryMock.Setup(repo => repo.FindAllSpec(It.IsAny<EmployeeSpec>()))
               .ReturnsAsync(employees);

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>())
                    .Returns(_employeeRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<IReadOnlyList<ReadEmployeeDto>>(employees)).Returns(readEmployeeDtos).Verifiable();
            // Act
            var result = await _employeesController.GetAllEmployeesAsync(pageSize, pageIndex, isPagingEnabled);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<Pagination<ReadEmployeeDto>>>(result);
        }

        #endregion

        #region GetById
        [Fact]
        public async Task GetEmployeeById()
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
        public async Task InsertEmployee()
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

        #region Update
        [Fact]
        public async Task UpdateEmployee()
        {
            // Arrange - Valid Employee
            var validEmployeeDto = new ReadEmployeeDto
            {
                Id = 1,
                EmpName = "Updated Employee",
                EmpType = 3,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(validEmployeeDto.EmpType))
                .Returns(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeAge(validEmployeeDto.EmpAge))
                .Returns(true)
                .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

            _mapperMock.Setup(m => m.Map<ReadEmployeeDto, Employee>(It.IsAny<ReadEmployeeDto>()))
                .Returns((ReadEmployeeDto dto) => new Employee
                {
                    Id = dto.Id,
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

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().UpdateAsync(It.IsAny<Employee>()))
                .Verifiable();

            _unitOfWorkMock.Setup(uof => uof.Commit())
                        .Returns(Task.FromResult(1));

            // Act - valid Employee
            var validResult = await _employeesController.UpdateEmployeeAsync(validEmployeeDto);

            // Assert - Valid Employee
            var okResult = validResult as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(validResult);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.UPDATED, apiResponseOkResult.Message);


            // Arrange - Invalid Employee Id
            var InvalidEmployeeDto = new ReadEmployeeDto
            {
                Id = 500,
                EmpName = "Updated Employee",
                EmpType = 3,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .Verifiable();

            // Act - Invalid Employee Id
            var invalidIdResult = await _employeesController.UpdateEmployeeAsync(InvalidEmployeeDto);

            // Assert - Invalid Employee Id
            var invalidIdNotFoundResult = invalidIdResult as NotFoundObjectResult;
            var invalidIdApiResponse = invalidIdNotFoundResult.Value as ApiResponse;

            Assert.NotNull(invalidIdResult);
            Assert.NotNull(invalidIdNotFoundResult);
            Assert.NotNull(invalidIdApiResponse);
            Assert.Equal(404, invalidIdApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidIdApiResponse.Message);


            // Arrange - Invalid type
            var InValidTypeEmployeeDto = new ReadEmployeeDto
            {
                Id = 1,
                EmpName = "Updated Employee",
                EmpType = 5,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeType(InValidTypeEmployeeDto.EmpType))
                .Returns(false)
                .Verifiable();

            // Act - Invalid type
            var errorResultInvalidType = await _employeesController.UpdateEmployeeAsync(InValidTypeEmployeeDto);

            // Assert - Invalid type
            var invalidTypeBadRequestResult = errorResultInvalidType as BadRequestObjectResult;
            var invalidTypeApiResponse = invalidTypeBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidType);
            Assert.NotNull(invalidTypeBadRequestResult);
            Assert.NotNull(invalidTypeApiResponse);
            Assert.Equal(400, invalidTypeBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_EMPTYPE, invalidTypeApiResponse.Message);


            // Arrange - Invalid age
            var InValidAgeEmployeeDto = new ReadEmployeeDto
            {
                Id = 1,
                EmpName = "Updated Employee",
                EmpType = 3,
                EmpAge = 0,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _employeeServicesMock.Setup(services => services.IsValidEmployeeAge(InValidAgeEmployeeDto.EmpAge))
                .Returns(false)
                .Verifiable();

            // Act - Invalid age
            var errorResultInvalidAge = await _employeesController.UpdateEmployeeAsync(InValidAgeEmployeeDto);

            // Assert - Invalid age
            var invalidAgeBadRequestResult = errorResultInvalidAge as BadRequestObjectResult;
            var invalidAgeApiResponse = invalidAgeBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidAge);
            Assert.NotNull(invalidAgeBadRequestResult);
            Assert.NotNull(invalidAgeApiResponse);
            Assert.Equal(400, invalidAgeBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_AGE, invalidAgeApiResponse.Message);



            // Arrange - Invalid phone number
            var InValidPhoneEmployeeDto = new ReadEmployeeDto
            {
                Id = 1,
                EmpName = "Updated Employee",
                EmpType = 3,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "011622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                .ReturnsAsync(true)
                .Verifiable();

            _phoneNumberValidatorMock.Setup(pnv => pnv.IsValidPhoneNumber(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act - Invalid phone number
            var errorResultInvalidPhoneNumber = await _employeesController.UpdateEmployeeAsync(InValidPhoneEmployeeDto);

            // Assert - Invalid phone number
            var invalidPhoneNumberBadRequestResult = errorResultInvalidPhoneNumber as BadRequestObjectResult;
            var invalidPhoneNumberApiResponse = invalidPhoneNumberBadRequestResult.Value as ApiResponse;

            Assert.NotNull(errorResultInvalidPhoneNumber);
            Assert.NotNull(invalidPhoneNumberBadRequestResult);
            Assert.NotNull(invalidPhoneNumberApiResponse);
            Assert.Equal(400, invalidPhoneNumberBadRequestResult.StatusCode);
            Assert.Equal(AppMessages.INVALID_PHONENUMBER, invalidPhoneNumberApiResponse.Message);

        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteEmployee()
        {
            // Arrange
            int employeeId = 1;
            var attendenceRepositoryMock = new Mock<IGenericRepository<Attendence>>();
            var payrollRepositoryMock = new Mock<IGenericRepository<Payroll>>();
            var vacationRepositoryMock = new Mock<IGenericRepository<Vacation>>();

            var attendences = new List<Attendence>
            {
                new Attendence { Id = 1, EmpId = employeeId, EmpArrivalTime = DateTime.Now , EmpLeavingTime = DateTime.Now.AddHours(8),
                                Permission = 0, DayDate = DateTime.Now, Month = 8}
            };

            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmpId = employeeId, SalaryDate = DateTime.Now , BasicSalary = 8000,
                                Bonus = 200, Deduct =100, TotalSalary = 8100}
            };

            var vacations = new List<Vacation>
            {
                new Vacation { Id = 1, EmpId = employeeId, DayDate = DateTime.Now, NormalVacation = false,
                                UrgentVacation = false , Absence = false}
            };

            var employee = new Employee
            {
                Id = employeeId,
                EmpName = "Rawan",
                EmpType = 3,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            var attendanceSpec = new AttendanceWithEmployeeSpec(null, employeeId);
            attendenceRepositoryMock.Setup(repo => repo.FindAllSpec(attendanceSpec))
                .ReturnsAsync(attendences)
                .Verifiable();

            var payrollSpec = new PayrollWithEmployeeSpec(null, employeeId);
            payrollRepositoryMock.Setup(repo => repo.FindAllSpec(payrollSpec))
                .ReturnsAsync(payrolls)
                .Verifiable();

            var vacationSpec = new VacationWithEmployeeSpec(null, employeeId);
            vacationRepositoryMock.Setup(repo => repo.FindAllSpec(vacationSpec))
                .ReturnsAsync(vacations)
                .Verifiable();

            _employeeRepositoryMock.Setup(repo => repo.FindSpec(It.IsAny<EmployeeSpec>()))
                .ReturnsAsync(employee)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Attendence>())
                .Returns(attendenceRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Payroll>())
                .Returns(payrollRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Vacation>())
                .Returns(vacationRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>())
                .Returns(_employeeRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act
            var result = await _employeesController.DeleteEmployeeAsync(employeeId.ToString());

            //Assert 
            if(attendences.Count > 0 || payrolls.Count > 0 || vacations.Count > 0)
            {
                var okResult = result as OkObjectResult;
                var apiResponseOkResult = okResult.Value as ApiResponse;

                Assert.NotNull(result);
                Assert.NotNull(okResult);
                Assert.NotNull(apiResponseOkResult);
                Assert.Equal(201, apiResponseOkResult.StatusCode);
                Assert.Equal(AppMessages.DELETED, apiResponseOkResult.Message);
            }
            else
            {
                var badRequestResult = result as BadRequestObjectResult;
                var apiResponseBadRequest = badRequestResult.Value as ApiResponse;

                Assert.Equal(400, apiResponseBadRequest.StatusCode);
                Assert.Equal(AppMessages.FAILED_DELETE, apiResponseBadRequest.Message);
            }
        }
        #endregion

        #region Fire
        [Fact]
        public async Task FireEmployee()
        {
            //Arrange - Valid Employee
            int employeeId = 1;
            var attendenceRepositoryMock = new Mock<IGenericRepository<Attendence>>();
            var payrollRepositoryMock = new Mock<IGenericRepository<Payroll>>();
            var vacationRepositoryMock = new Mock<IGenericRepository<Vacation>>();

            var attendences = new List<Attendence>
            {
                new Attendence { Id = 1, EmpId = employeeId, EmpArrivalTime = DateTime.Now , EmpLeavingTime = DateTime.Now.AddHours(8),
                                Permission = 0, DayDate = DateTime.Now, Month = 8}
            };

            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmpId = employeeId, SalaryDate = DateTime.Now , BasicSalary = 8000,
                                Bonus = 200, Deduct =100, TotalSalary = 8100}
            };

            var vacations = new List<Vacation>
            {
                new Vacation { Id = 1, EmpId = employeeId, DayDate = DateTime.Now, NormalVacation = false,
                                UrgentVacation = false , Absence = false}
            };

            var employee = new Employee
            {
                Id = employeeId,
                EmpName = "Rawan",
                EmpType = 3,
                EmpAge = 21,
                EmpAddress = "Cairo",
                EmpPhoneNumber = "01013451622",
                EmpStartingShift = DateTime.Now,
                EmpEndingShift = DateTime.Now.AddHours(8),
                EmpBasicSalary = 8000
            };

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Employee>())
                .Returns(_employeeRepositoryMock.Object)
                .Verifiable();

            _employeeRepositoryMock.Setup(repo => repo.Exists(employeeId))
                    .ReturnsAsync(true)
                    .Verifiable();

            _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(employeeId))
                    .ReturnsAsync(employee);

            var attendanceSpec = new AttendanceWithEmployeeSpec(null, employeeId);
            attendenceRepositoryMock.Setup(repo => repo.FindAllSpec(attendanceSpec))
                .ReturnsAsync(attendences)
                .Verifiable();

            var payrollSpec = new PayrollWithEmployeeSpec(null, employeeId);
            payrollRepositoryMock.Setup(repo => repo.FindAllSpec(payrollSpec))
                .ReturnsAsync(payrolls)
                .Verifiable();

            var vacationSpec = new VacationWithEmployeeSpec(null, employeeId);
            vacationRepositoryMock.Setup(repo => repo.FindAllSpec(vacationSpec))
                .ReturnsAsync(vacations)
                .Verifiable();

            attendenceRepositoryMock.Setup(repo => repo.DeleteRangeAsync(attendences)).Verifiable();

            payrollRepositoryMock.Setup(repo => repo.DeleteRangeAsync(payrolls)).Verifiable();

            vacationRepositoryMock.Setup(repo => repo.DeleteRangeAsync(vacations)).Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Attendence>())
                .Returns(attendenceRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Payroll>())
                .Returns(payrollRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(uow => uow.GetRepository<Vacation>())
                .Returns(vacationRepositoryMock.Object)
                .Verifiable();

            
            _unitOfWorkMock.Setup(uow => uow.Commit())
                .Returns(Task.FromResult(1));

            // Act - Valid Employee
            var result = await _employeesController.FireEmployeeAsync(employeeId.ToString());

            //Assert - Valid Employee
            var okResult = result as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.FIRED, apiResponseOkResult.Message);



            // Arrange - Invalid Employee Id
            int Invalid_employeeId = 1;

            _unitOfWorkMock.Setup(uof => uof.GetRepository<Employee>().Exists(It.IsAny<int>()))
                    .ReturnsAsync(false)
                    .Verifiable();

            // Act - Invalid Employee Id
            var invalidIdResult = await _employeesController.FireEmployeeAsync(Invalid_employeeId.ToString());

            // Assert - Invalid Employee Id
            var invalidIdNotFoundResult = invalidIdResult as NotFoundObjectResult;
            var invalidIdApiResponse = invalidIdNotFoundResult.Value as ApiResponse;

            Assert.NotNull(invalidIdResult);
            Assert.NotNull(invalidIdNotFoundResult);
            Assert.NotNull(invalidIdApiResponse);
            Assert.Equal(404, invalidIdApiResponse.StatusCode);
            Assert.Equal(AppMessages.INVALID_ID, invalidIdApiResponse.Message);
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
