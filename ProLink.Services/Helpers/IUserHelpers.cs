using Microsoft.AspNetCore.Http;
using ProLink.Data.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ProLink.Application.Helpers
{
    public interface IUserHelpers
    {
        Task<JwtSecurityToken> GenerateJwtTokenAsync(/*IEnumerable<Claim> claims*/User user);
        Task<User> GetCurrentUserAsync();
        Task<string> AddFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string imagePath, string folderName);
    }
}
