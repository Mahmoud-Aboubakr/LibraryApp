using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Handlers;
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

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

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
                return NotFound(new ApiResponse(404));

            return Ok(registerData);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRegisterDataByEmail(string email, UpdateRegisterDataDto updatedData)
        {
            var updatedRegisterData = await _authService.UpdateUserRegisterDataByEmailAsync(email, updatedData);

            if (updatedRegisterData == null)
                return NotFound(new ApiResponse(404));

            return Ok(updatedRegisterData);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserByEmailAsync(string email)
        {
            var deletedData = await _authService.DeleteUserDataByEmailAsync(email);

            if (!deletedData)
                return BadRequest(new ApiResponse(400));

            return Ok(new ApiResponse(201, AppMessages.DELETED));
        }
    }
}
