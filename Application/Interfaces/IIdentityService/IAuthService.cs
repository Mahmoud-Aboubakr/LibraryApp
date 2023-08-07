using Application.DTOs.Identity;
using Application.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IIdentityService
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<List<RegisterDataDto>> GetAllRegisterDataAsync();
        Task<RegisterDataDto> GetRegisterDataByEmailAsync(string email);
        Task<UpdateRegisterDataDto> UpdateUserRegisterDataByEmailAsync(string email, UpdateRegisterDataDto updatedData);
        Task<bool> DeleteUserDataByEmailAsync(string email);
    }
}
