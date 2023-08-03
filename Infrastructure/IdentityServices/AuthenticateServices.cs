using Application.DTOs.Author;
using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Handlers;
using Application.Interfaces.IIdentityService;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityServices
{
    public class AuthenticateServices : IAuthenticateServices
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateServices(IConfiguration configuration, IMapper mapper ,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<AuthDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
                throw new BadRequestException(AppMessages.EXISTING_EMAIL);

            if (await _userManager.FindByEmailAsync(registerDto.Username) is not null)
                throw new BadRequestException(AppMessages.EXISTING_EMAIL);

            var user = _mapper.Map<RegisterDto, Users>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthDto { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateToken(user);

            return new AuthDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<AuthDto> LoginAsync(LoginDto loginDto)
        {
            var authDto = new AuthDto();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if(user is null )
            {
                throw new BadRequestException(AppMessages.INVALID_LOGIN);
            }
            if( !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new BadRequestException(AppMessages.WRONG_PASS);
            }
            var jwtSecurityToken = await CreateToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authDto.Email = user.Email;
            authDto.ExpiresOn = jwtSecurityToken.ValidTo;
            authDto.IsAuthenticated = true;
            authDto.Roles = rolesList.ToList();
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDto.Username = user.UserName;

            return authDto;
        }

        public async Task<JwtSecurityToken> CreateToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                 new Claim("uid", user.Id),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
              (
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials              
              );

            return jwtSecurityToken;
        }

        public async Task<string> AddRoleAsync(RoleDto roleDto)
        {
            var user = await _userManager.FindByIdAsync(roleDto.UserId);
            if (user is null )
                throw new BadRequestException(AppMessages.INVALID_LOGIN);

            if (!await _roleManager.RoleExistsAsync(roleDto.Role))
                throw new BadRequestException(AppMessages.INVALID_ROLE);

            if(await _userManager.IsInRoleAsync(user,roleDto.Role))
                throw new BadRequestException(AppMessages.EXISTING_ROLE);

            var result = await _userManager.AddToRoleAsync(user, roleDto.Role);

            return result.Succeeded ? string.Empty : throw new BadRequestException(AppMessages.BAD_REQUEST);
        }
    }
}
