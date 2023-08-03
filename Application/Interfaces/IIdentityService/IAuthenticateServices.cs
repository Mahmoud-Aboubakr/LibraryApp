using Application.DTOs.Identity;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IIdentityService
{
    public interface IAuthenticateServices
    {
        Task<AuthDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthDto> LoginAsync(LoginDto loginDto);
        Task<string> AddRoleAsync(RoleDto roleDto);
        Task<JwtSecurityToken> CreateToken(IdentityUser user);

    }
}
