//using API.Controllers;
//using Application;
//using Application.DTOs.Author;
//using Application.Interfaces;
//using Application.Interfaces.IAppServices;
//using Application.Interfaces.IValidators;
//using Application.Validators;
//using AutoMapper;
//using Infrastructure.AppServices;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Persistence.Context;
//using Persistence.Repositories;

//namespace API.Test
//{
//    public class AuthorsControllerTest1
//    {
//        LibraryDbContext _context;
//        IConfigurationProvider _config;
//        ILoggerFactory _factory;
//        IUnitOfWork _uof;
//        IMapper _mapper;
//        IPhoneNumberValidator _phoneNumberValidator;
//        IAuthorServices _authorServices;
//        ILogger<AuthorsController> _logger;
//        AuthorsController authorsController;

//        public AuthorsControllerTest1()
//        {
//            _uof = new UnitOfWork(_context);
//            _mapper = new Mapper(_config);
//            _phoneNumberValidator = new PhoneNumberValidator();
//            _authorServices = new AuthorServices(_context, _mapper);
//            _logger = new Logger<AuthorsController>(_factory);
//            authorsController = new AuthorsController(_uof,
//                _mapper,
//                _phoneNumberValidator,
//                _authorServices,
//                _logger);
//        }


//        [Fact]
//        public void GetAllAuthorsAsync()
//        {
//            //Arrange
//            //Act
//            var result = authorsController.GetAllAuthorsAsync();
//            //Assert
//            Assert.NotNull(result);
//        }
//    }
//}