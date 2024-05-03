using Microsoft.AspNetCore.Http;
using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetCurrentUserInfoAsync();
        Task<bool> UpdateUserInfoAsync(UserDto userDto);
        Task<bool> DeleteAccountAsync();
        Task<bool> AddUserPictureAsync(IFormFile file);
        Task<bool> DeleteUserPictureAsync();
        Task<bool> UpdateUserPictureAsync(IFormFile? file);
        Task<string> GetUserPictureAsync();
    }
}
