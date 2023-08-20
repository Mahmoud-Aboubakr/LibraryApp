using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Handlers;
using Application.IdentityModels;
using Application.Interfaces.IIdentityService;
using Domain.Constants;
using Domain.Entities.Identity;
using Infrastructure.IdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Register
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            _authService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        #endregion

        #region Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                _authService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        #endregion

        #region AddRole
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        #endregion

        #region Get
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserRegisterData()
        {
            var registerData = await _authService.GetAllRegisterDataAsync();

            return Ok(registerData);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetUserRegisterDataByEmail(string email)
        {
            var registerData = await _authService.GetRegisterDataByEmailAsync(email);

            if (registerData == null)
                return NotFound(new ApiResponse(404 , AppMessages.NOTFOUND_USER));

            return Ok(registerData);
        }
        #endregion

        #region Update
        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRegisterDataByEmail(string email, UpdateRegisterDataDto updatedData)
        {
            var updatedRegisterData = await _authService.UpdateUserRegisterDataByEmailAsync(email, updatedData);

            if (updatedRegisterData == null)
                return NotFound(new ApiResponse(404 , AppMessages.NOTFOUND_USER));

            return Ok(updatedRegisterData);
        }
        #endregion

        #region Delete
        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserByEmailAsync(string email)
        {
            var deletedData = await _authService.DeleteUserDataByEmailAsync(email);

            if (!deletedData)
                return BadRequest(new ApiResponse(400 , AppMessages.FAILDE_USER_DELETE));

            return Ok(new ApiResponse(201, AppMessages.DELETED));
        }
        #endregion

        #region RefreshToken
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(new ApiResponse(400 , AppMessages.UNAUTHENTICATED));

            _authService.SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        #endregion

        #region RevokeToken
        [HttpPost]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeModel model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                throw new BadRequestException(AppMessages.REQUIRED_TOKEN);

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                throw new BadRequestException(AppMessages.INVALID_TOKEN);

            return Ok();
        }
        #endregion
    }
}
