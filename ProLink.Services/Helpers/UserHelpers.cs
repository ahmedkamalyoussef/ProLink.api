using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProLink.Application.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ProLink.Application.Consts;

namespace ProLink.Application.Helpers
{
    internal class UserHelpers : IUserHelpers
    {
        #region fields
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;
        #endregion

        #region ctor
        public UserHelpers(IConfiguration config, UserManager<User> userManager
            , IHttpContextAccessor contextAccessor
            , IWebHostEnvironment webHostEnvironment
            , AppDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        #endregion

        #region methods
        public async Task<User> GetCurrentUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }

        public async Task<LoginResult> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResult
            {
                Success = true,
                Token = tokenString,
                Expiration = token.ValidTo
            };
        }
        #endregion

        #region file handling
        public async Task<string> AddFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;
            string profileFolderPath = "";
            if (folderName == ConstsFiles.CV)
                profileFolderPath = Path.Combine(rootPath, "CV", userName);
            else
                profileFolderPath = Path.Combine(rootPath, "Images", userName, folderName);
            if (!Directory.Exists(profileFolderPath))
            {
                Directory.CreateDirectory(profileFolderPath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(profileFolderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            if (folderName == ConstsFiles.CV)
                return $"/CV/{userName}/{fileName}";
            return $"/Images/{userName}/{folderName}/{fileName}";

        }

        public async Task<bool> DeleteFileAsync(string filePath, string folderName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("file path is null or empty.", nameof(filePath));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;

            if (folderName == ConstsFiles.CV)
            {
                if (!filePath.StartsWith($"/CV/{userName}/"))
                {
                    throw new ArgumentException("Invalid file path.", nameof(filePath));
                }
            }

            else
            {
                if (!filePath.StartsWith($"/Images/{userName}/{folderName}/"))
                {
                    throw new ArgumentException("Invalid file path.", nameof(filePath));
                }
            }
            string fullFilePath = Path.Combine(rootPath, filePath.TrimStart('/'));

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
                return true;
            }
            else
            {
                throw new FileNotFoundException("File not found.", fullFilePath);
            }

        }
        #endregion
    }
}