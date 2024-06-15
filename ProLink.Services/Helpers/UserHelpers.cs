using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
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
        #endregion

        #region ctor
        public UserHelpers(IConfiguration config, UserManager<User> userManager
            , IHttpContextAccessor contextAccessor
            , IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region methods
        public async Task<User> GetCurrentUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.Now.AddMinutes(15);
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region file handling
        public async Task<string> AddFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
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
                return true;
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