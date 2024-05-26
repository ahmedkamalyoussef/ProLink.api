using Microsoft.AspNetCore.Http;
using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResultDto> GetCurrentUserInfoAsync();
        Task<UserResultDto> GetUserByIdAsync(string id);
        Task<List<UserResultDto>> GetUsersByNameAsync(string name);
        Task<bool> UpdateUserInfoAsync(UserDto userDto);
        Task<bool> DeleteAccountAsync();
        Task<bool> DeleteUserPictureAsync();
        Task<bool> UpdateUserPictureAsync(/*IFormFile? file*/ string path);
        Task<string> GetUserPictureAsync();
        Task<bool> DeleteUserCVAsync();
        Task<bool> UpdateUserCVAsync(/*IFormFile? file*/ string path);

        Task<string> GetUserCVAsync();
        Task<bool> DeleteUserBackImageAsync();
        Task<bool> UpdateUserBackImageAsync(/*IFormFile? file*/ string path);

        Task<string> GetUserBackImageAsync();
       
    }
}
