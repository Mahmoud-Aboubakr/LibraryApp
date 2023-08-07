using Application.DTOs.Identity;
using Application.Exceptions;
using Application.Interfaces.IIdentityService;
using AutoMapper;
using Domain.Constants;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Application.IdentityModels;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.IdentityServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IMapper mapper,
            IOptions<JWT> jwt,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwt = jwt.Value;
        }

        #region Register
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                throw new BadRequestException(AppMessages.REGISTERED_EMAIL);

            if (await _userManager.FindByNameAsync(model.Username) is not null)
                throw new BadRequestException(AppMessages.REGESTERED_USER);

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                //ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }
        #endregion

        #region GetToken
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new BadRequestException(AppMessages.INVALID_CREDIENTIALS);
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            //authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;
        }
        #endregion

        #region AddRole
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                throw new BadRequestException(AppMessages.INVALID_IDorRole);

            if (await _userManager.IsInRoleAsync(user, model.Role))
                throw new BadRequestException(AppMessages.ASSIGNED_ROLE);

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : throw new BadRequestException(AppMessages.WRONG);
        }
        #endregion

        #region CreateJwtToken
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        #endregion

        #region Get
        public async Task<List<RegisterDataDto>> GetAllRegisterDataAsync()
        {
            var users = _userManager.Users.ToList();

            var registerDataList = new List<RegisterDataDto>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var registerData = new RegisterDataDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Password = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userRoles.ToList()
                };

                registerDataList.Add(registerData);
            }

            return registerDataList;
        }

        public async Task<RegisterDataDto> GetRegisterDataByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new BadRequestException(AppMessages.INVALID_EMAIL);

            var userRoles = await _userManager.GetRolesAsync(user);

            var registerData = new RegisterDataDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                Roles = userRoles.ToList()
            };

            return registerData;
        }
        #endregion

        #region Update
        public async Task<UpdateRegisterDataDto> UpdateUserRegisterDataByEmailAsync(string email, UpdateRegisterDataDto updatedData)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new BadRequestException(AppMessages.INVALID_EMAIL);

            user.UserName = updatedData.Username;
            user.Email = updatedData.Email;
            user.PasswordHash = _passwordHasher.HashPassword(user, updatedData.Password);
            user.PhoneNumber = updatedData.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);


            if (!result.Succeeded)
                throw new BadRequestException();

            if (updatedData.Roles != null && updatedData.Roles.Count > 0)
            {
                var existingRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = existingRoles.Except(updatedData.Roles);
                var rolesToAdd = updatedData.Roles.Except(existingRoles);

                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            return updatedData;
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteUserDataByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new BadRequestException(AppMessages.INVALID_EMAIL);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new BadRequestException();

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            return true;
        }
        #endregion

        #region RefreshToken
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(7),
                CreatedOn = DateTime.Now
            };
        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = AppMessages.INVALID_TOKEN;
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = AppMessages.INACTIVE_TOKEN;
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.Now;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        #endregion

        #region RevokeToken
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);

            return true;
        }
        #endregion

    }
}
