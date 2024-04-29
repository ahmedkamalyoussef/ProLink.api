using Castle.Core.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;
using ProLink.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterUser registerUser);
        Task<bool> ConfirmEmailAsync(string email, string token);
        Task<LoginResult> LoginAsync(LoginUser loginUser);
        Task<LogoutResult> LogoutAsync();
        Task<bool> ForgetPasswordAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword);
        Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword);
        Task<bool> UpdateUserInfoAsync(UserDto userDto);
        Task<bool> DeleteAccountAsync();
        Task<bool> AddUserPictureAsync(IFormFile file);
        Task<bool> DeleteUserPictureAsync();
        Task<bool> UpdateUserPictureAsync(IFormFile? file);
        Task<string> GetUserPictureAsync();
    }
}
