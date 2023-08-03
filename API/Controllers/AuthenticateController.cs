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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticateServices _authenticateServices;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuthenticateServices authenticateServices)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticateServices = authenticateServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(AppMessages.BAD_REQUEST);

            var result = await _authenticateServices.RegisterAsync(registerDto);

            if (!result.IsAuthenticated)
                throw new BadRequestException(result.Message);

            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(AppMessages.BAD_REQUEST);

            var result = await _authenticateServices.LoginAsync(loginDto);

            if(!result.IsAuthenticated)
                throw new BadRequestException(result.Message);

            return Ok(result);

        }
        [Authorize(Roles ="Manager")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(AppMessages.BAD_REQUEST);

            var result = await _authenticateServices.AddRoleAsync(roleDto);

            if (!string.IsNullOrEmpty(result))
                throw new BadRequestException(result);

            return Ok(roleDto);
        }
    }
}
