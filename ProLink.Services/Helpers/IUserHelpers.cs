using Microsoft.AspNetCore.Http;
using ProLink.Application.Authentication;
using ProLink.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Helpers
{
    public interface IUserHelpers
    {
        Task<Login> GenerateJwtTokenAsync(IEnumerable<Claim> claims);
        Task<User> GetCurrentUserAsync();
        Task<string> AddImage(IFormFile file);
        Task DeleteImageAsync(string imagePath);
    }
}
