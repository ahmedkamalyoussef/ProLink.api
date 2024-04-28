using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace ProLink.Core.Helpers
{
    internal class UserHelpers:IUserHelpers
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;


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
        public async Task<User> GetCurrentUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
    }
}
