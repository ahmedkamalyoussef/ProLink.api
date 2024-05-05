using Microsoft.AspNetCore.Http;
using ProLink.Application.Authentication;
using ProLink.Data.Entities;
using System.Security.Claims;

namespace ProLink.Application.Helpers
{
    public interface IUserHelpers
    {
        Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
        Task<User> GetCurrentUserAsync();
        Task<string> AddFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string imagePath, string folderName);
    }
}
