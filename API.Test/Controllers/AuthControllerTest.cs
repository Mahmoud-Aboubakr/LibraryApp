using API.Controllers;
using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Handlers;
using Application.IdentityModels;
using Application.Interfaces.IIdentityService;
using Domain.Constants;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Test.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _authServiceMock = new Mock<IAuthService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        #region Register
        [Fact]
        public async Task Register()
        {
            // Arrange validModel
            var validModel = new RegisterModel
            {
                Email = "test@example.com",
                Username = "string",
                Password = "String@1"
            };
            var authModel = new AuthModel
            {
                Email = "test@example.com",
                IsAuthenticated = true,
                Token = "token",
                RefreshToken = "refreshToken",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };
            _authServiceMock.Setup(service => service.RegisterAsync(validModel)).ReturnsAsync(authModel);

            // Act validModel
            var result = await _authController.RegisterAsync(validModel);

            // Assert validModel
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var model = Assert.IsType<AuthModel>(okResult.Value);
            Assert.Equal(authModel.Email, model.Email);
            Assert.Equal(authModel.IsAuthenticated, model.IsAuthenticated);
            Assert.Equal(authModel.Token, model.Token);
            Assert.Equal(authModel.RefreshToken, model.RefreshToken);
            Assert.Equal(authModel.RefreshTokenExpiration, model.RefreshTokenExpiration);

            _authServiceMock.Verify(service => service.SetRefreshTokenInCookie(authModel.RefreshToken, authModel.RefreshTokenExpiration), Times.Once);

            //Arrange invalidModel
            var invalidModel = new RegisterModel
            {
                Email = "test@example.com",
                Username = "string",
                Password = "String@1"
            };
            var errors = string.Empty;
            var authModel2 = new AuthModel
            {
                Email = "test@example.com",
                IsAuthenticated = true,
                Token = "token",
                RefreshToken = "refreshToken",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };
            _authController.ModelState.AddModelError(invalidModel.Email, AppMessages.REGISTERED_EMAIL);
            _authController.ModelState.AddModelError(invalidModel.Username, AppMessages.REGESTERED_USER);
            // Act invalidModel
            var badResult = await _authController.RegisterAsync(invalidModel);

            // Assert invalidModel
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region Login
        [Fact]
        public async Task Login()
        {
            // Arrange valid
            var validModel = new TokenRequestModel
            {
                Email = "testemail",
                Password = "TestPassword"
            };
            var authModel = new AuthModel
            {
                Email = "test@example.com",
                IsAuthenticated = true,
                Token = "token",
                RefreshToken = "refreshToken",
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7)
            };
            _authServiceMock.Setup(service => service.GetTokenAsync(validModel)).ReturnsAsync(authModel);

            // Act valid
            var result = await _authController.Login(validModel);

            // Assert valid
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var model = Assert.IsType<AuthModel>(okResult.Value);
            Assert.Equal(authModel.Email, model.Email);
            Assert.Equal(authModel.IsAuthenticated, model.IsAuthenticated);
            Assert.Equal(authModel.Token, model.Token);
            Assert.Equal(authModel.RefreshToken, model.RefreshToken);
            Assert.Equal(authModel.RefreshTokenExpiration, model.RefreshTokenExpiration);

            if (!string.IsNullOrEmpty(authModel.RefreshToken))
            {
                _authServiceMock.Verify(service => service.SetRefreshTokenInCookie(authModel.RefreshToken, authModel.RefreshTokenExpiration), Times.Once);
            }
            else
            {
                _authServiceMock.Verify(service => service.SetRefreshTokenInCookie(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            }

            // Arrange invalid
            var invalidModel = new TokenRequestModel
            {
                Email = "testemail",
                Password = "123"
            };

            _authController.ModelState.AddModelError(invalidModel.Email, AppMessages.INVALID_CREDIENTIALS);
            _authController.ModelState.AddModelError(invalidModel.Password, AppMessages.INVALID_CREDIENTIALS);

            // Act invalid
            var badResult = await _authController.Login(invalidModel);

            // Assert invalid
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion

        #region AddRole
        [Fact]
        public async Task AddRoleAsync_WhenRoleAlreadyExists()
        {
            // Arrange
            var model = new AddRoleModel
            {
                Role = "1",
                UserId = "1"
            };
            _authController.ModelState.AddModelError(model.UserId, AppMessages.ASSIGNED_ROLE);

            // Act
            var result = await _authController.AddRoleAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task AddRoleAsync_WhenErrorModelIsRole()
        {
            // Arrange
            var model = new AddRoleModel
            {
                Role = "1",
                UserId = "1"
            };
            _authController.ModelState.AddModelError(model.Role, AppMessages.INVALID_IDorRole);

            // Act
            var result = await _authController.AddRoleAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task AddRoleAsync_WhenErrorModelIsUser()
        {
            // Arrange
            var model = new AddRoleModel
            {
                Role = "1",
                UserId = "1"
            };
            _authController.ModelState.AddModelError(model.UserId, AppMessages.INVALID_IDorRole);

            // Act
            var result = await _authController.AddRoleAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task AddRoleAsync_ShouldReturnOk()
        {
            // Arrange
            var model = new AddRoleModel
            {
                Role = "1",
                UserId = "1"
            };
            _authServiceMock.Setup(s => s.AddRoleAsync(model)).ReturnsAsync((string)null);

            // Act
            var result = await _authController.AddRoleAsync(model);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(model, okResult.Value);
        }
        #endregion

        #region GetAllUsers
        [Fact]
        public async Task GetAllUserRegisterData_ShouldReturnOk_WithRegisterData()
        {
            // Arrange
            var registerData = new List<RegisterDataDto>
            {
                new RegisterDataDto
                {
                Email = "test@example.com",
                Username = "string",
                Password  = "String@1"
                },
                new RegisterDataDto {
                    Email = "test2@example.com",
                Username = "string2",
                Password  = "String@12"
                }
            };
            _authServiceMock.Setup(s => s.GetAllRegisterDataAsync()).ReturnsAsync(registerData);

            // Act
            var result = await _authController.GetAllUserRegisterData();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(registerData, okResult.Value);
        }
        #endregion

        #region GetUserByEmail
        [Fact]
        public async Task GetUserRegisterDataByEmail_NotFound()
        {
            // Arrange
            var email = "invalid@test.com";
            _authServiceMock.Setup(s => s.GetRegisterDataByEmailAsync(email)).ReturnsAsync((RegisterDataDto)null);

            // Act
            var result = await _authController.GetUserRegisterDataByEmail(email);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(AppMessages.NOTFOUND_USER, (notFoundResult.Value as ApiResponse).Message);
        }

        [Fact]
        public async Task GetUserRegisterDataByEmail_Ok()
        {
            // Arrange
            var email = "test@test.com";
            var registerData = new RegisterDataDto
            {
                Email = "test@example.com",
                Username = "string",
                Password = "String@1"
            };
            _authServiceMock.Setup(s => s.GetRegisterDataByEmailAsync(email)).ReturnsAsync(registerData);

            // Act
            var result = await _authController.GetUserRegisterDataByEmail(email);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(registerData, okResult.Value);
        }
        #endregion

        #region Update

        [Fact]
        public async Task UpdateUserRegisterDataByEmail()
        {
            // Arrange valid
            var email = "test@test.com";
            var updatedData = new UpdateRegisterDataDto
            {
                Email = "test@example.com",
                Username = "string",
                Password = "String@1"
            };
            _authServiceMock.Setup(s => s.UpdateUserRegisterDataByEmailAsync(email, updatedData)).ReturnsAsync(updatedData);

            // Act valid
            var result = await _authController.UpdateUserRegisterDataByEmail(email, updatedData);

            // Assert valid
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(updatedData, okResult.Value);

            // Arrange invalid
            var invalidEmail = "invalid@test.com";
            var invalidUpdatedData = new UpdateRegisterDataDto
            {
                Email = "test22@example.com",
                Username = "string",
                Password = "String@1"
            };

            // Act invalid
            var badResult = await _authController.UpdateUserRegisterDataByEmail(invalidEmail, invalidUpdatedData);

            // Assert invalid
            Assert.IsType<NotFoundObjectResult>(badResult);
            var notFoundResult = badResult as NotFoundObjectResult;
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(AppMessages.NOTFOUND_USER, (notFoundResult.Value as ApiResponse).Message);
        }

        #endregion

        #region Delete
        [Fact]
        public async Task DeleteUserByEmailAsync()
        {
            // Arrange valid
            var email = "test@test.com";
            _authServiceMock.Setup(s => s.DeleteUserDataByEmailAsync(email)).ReturnsAsync(true);

            // Act valid
            var result = await _authController.DeleteUserByEmailAsync(email);

            // Assert valid
            var okResult = result as OkObjectResult;
            var apiResponseOkResult = okResult.Value as ApiResponse;

            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.NotNull(apiResponseOkResult);
            Assert.Equal(201, apiResponseOkResult.StatusCode);
            Assert.Equal(AppMessages.DELETED, apiResponseOkResult.Message);

            // Arrange invalid
            var invalidEmail = "test@test.com";
            _authServiceMock.Setup(s => s.DeleteUserDataByEmailAsync(invalidEmail)).ReturnsAsync(false);

            // Act invalid
            var badResult = await _authController.DeleteUserByEmailAsync(invalidEmail);

            // Assert invalid
            Assert.IsType<BadRequestObjectResult>(badResult);
            var badRequestResult = badResult as BadRequestObjectResult;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(AppMessages.FAILDE_USER_DELETE, (badRequestResult.Value as ApiResponse).Message);
        }
        #endregion

        #region RefreshToken
        [Fact]
        public async Task RefreshToken()
        {
            // Arrange Ok
            var refreshToken = "valid-token";
            var result = new AuthModel
            {
                IsAuthenticated = true,
                Token = "new-token",
                RefreshToken = "new-refresh-token",
                RefreshTokenExpiration = DateTime.Now.AddDays(7)
            };
            _authServiceMock.Setup(s => s.RefreshTokenAsync(refreshToken)).ReturnsAsync(result);

            var requestCookiesMock = new Mock<IRequestCookieCollection>();
            requestCookiesMock.Setup(r => r["refreshToken"]).Returns(refreshToken);
            var responseCookiesMock = new Mock<IResponseCookies>();
            _httpContextAccessorMock.Setup(r => r.HttpContext.Request.Cookies).Returns(requestCookiesMock.Object);
            _httpContextAccessorMock.Setup(r => r.HttpContext.Response.Cookies).Returns(responseCookiesMock.Object);

            _authController.ControllerContext.HttpContext = _httpContextAccessorMock.Object.HttpContext;

            // Act Ok
            var response = await _authController.RefreshToken();

            // Assert Ok
            Assert.IsType<OkObjectResult>(response);
            var okResult = response as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(result, okResult.Value);

            // Arrange Bad
            var invalidToken = "invalid-token";
            var authModel = new AuthModel
            {
                IsAuthenticated = false
            };
            _authServiceMock.Setup(s => s.RefreshTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(authModel);
            _httpContextAccessorMock.Setup(h => h.HttpContext.Request.Cookies["refreshToken"])
                .Returns(refreshToken);
            _authController.ControllerContext = new ControllerContext();
            _authController.ControllerContext.HttpContext = _httpContextAccessorMock.Object.HttpContext;

            // Act Bad
            var badResult = await _authController.RefreshToken();

            // Assert Bad
            Assert.IsType<BadRequestObjectResult>(badResult);
            var badRequestResult = badResult as BadRequestObjectResult;
            Assert.IsType<ApiResponse>(badRequestResult.Value);
            var apiResponse = badRequestResult.Value as ApiResponse;
            Assert.Equal(400, apiResponse.StatusCode);
            Assert.Equal(AppMessages.UNAUTHENTICATED, apiResponse.Message);
        }
        #endregion

        #region RevokeToken
        [Fact]
        public async Task RevokeToken_ReturnsOk_WhenTokenIsValid()
        {
            // Arrange
            var model = new RevokeModel
            {
                Token = "valid-Token" 
            };
            _authServiceMock.Setup(s => s.RevokeTokenAsync(model.Token))
                .ReturnsAsync(true);

            var requestCookiesMock = new Mock<IRequestCookieCollection>();
            requestCookiesMock.Setup(r => r["refreshToken"]).Returns(model.Token);
            var responseCookiesMock = new Mock<IResponseCookies>();
            _httpContextAccessorMock.Setup(h => h.HttpContext.Request.Cookies)
                .Returns(requestCookiesMock.Object);
            _httpContextAccessorMock.Setup(h => h.HttpContext.Response.Cookies)
                .Returns(responseCookiesMock.Object);
            _authController.ControllerContext = new ControllerContext();
            _authController.ControllerContext.HttpContext = _httpContextAccessorMock.Object.HttpContext;

            // Act
            var result = await _authController.RevokeToken(model);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        #endregion
    }
}
