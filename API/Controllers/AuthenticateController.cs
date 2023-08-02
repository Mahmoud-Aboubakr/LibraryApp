using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Handlers;
using Application.Interfaces.IIdentityService;
using Domain.Constants;
using Domain.Entities.Identity;
using Infrastructure.IdentityServices;
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto , string role)
        {
            if (ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(registerDto.Email);
                if (user_exist != null)
                {
                    throw new BadHttpRequestException(AppMessages.EXISTING_EMAIL);
                }
                var new_user = new IdentityUser()
                {
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    PhoneNumber = registerDto.PhoneNumber,
                    EmailConfirmed = registerDto.EmailConfirmed,
                    PhoneNumberConfirmed = registerDto.PhoneNumberConfirmed,
                };
                var is_created = await _userManager.CreateAsync(new_user, registerDto.Password);
                if (is_created.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    else
                        await _userManager.AddToRoleAsync(new_user, role);
                    var token = _authenticateServices.CreateToken(new_user);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
                }
                 throw new BadRequestException(AppMessages.BAD_REQUEST);
            }
            throw new BadRequestException(AppMessages.BAD_REQUEST);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var existing_user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (existing_user == null)
                    throw new NotFoundException(AppMessages.INVALID_LOGIN);
                var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginDto.Password);
                if (!isCorrect)
                    throw new BadRequestException(AppMessages.INVALID_PAYLOAD);
                var jwtToken = _authenticateServices.CreateToken(existing_user);
                return Ok(new AuthResult()
                {
                    Token = jwtToken,
                    Result = true
                });
            }

            throw new BadRequestException(AppMessages.INVALID_PAYLOAD);

        }
    }
}
