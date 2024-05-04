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
        Task<bool> AddUserPictureAsync(IFormFile file);
        Task<bool> DeleteUserPictureAsync();
        Task<bool> UpdateUserPictureAsync(IFormFile? file);
        Task<string> GetUserPictureAsync();

        Task<bool> AddSkillAsync(AddSkillDto addSkilltDto);
        Task<List<SkillDto>> GetCurrentUserSkillsAsync();
        Task<List<SkillDto>> GetUserSkillsByIdAsync(string id);
        Task<bool> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto);
        Task<bool> DeleteSkillAsync(string skillId);
        Task<bool> AddRateAsync(string userId, RateDto rateDto);
        Task<bool> DeleteRateAsync(string userId);
    }
}
