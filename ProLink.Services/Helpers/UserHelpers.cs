using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProLink.Application.Authentication;
using System.Security.Claims;

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

        public Task<Login> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region file handling
        public async Task<string> AddImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.", nameof(file));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;
            string profileFolderPath = Path.Combine(rootPath, "Images", userName);
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

            return $"/Images/{userName}/{fileName}";
        }

        public async Task DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Image path is null or empty.", nameof(imagePath));
            }

            string rootPath = _webHostEnvironment.WebRootPath;
            var user = await GetCurrentUserAsync();
            string userName = user.UserName;


            if (!imagePath.StartsWith($"/Images/{userName}/"))
            {
                throw new ArgumentException("Invalid image path.", nameof(imagePath));
            }

            string filePath = Path.Combine(rootPath, imagePath.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
        }
        #endregion
    }
}