using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Castle.Core.Resource;
using System.Net.Mail;
using ProLink.Application.Authentication;
using ProLink.Application.Consts;

namespace ProLink.Application.Services
{
    public class UserService : IUserService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly SignInManager<User> _signInManager;
        #endregion

        #region ctor
        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IConfiguration config, IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUserHelpers userHelpers,
            SignInManager<User> signInManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _signInManager = signInManager;
        }
        #endregion
        #region Registration
        public async Task<IdentityResult> Register(RegisterUser registerUser)
        {
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);

            if (userExist != null)
                return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });

            var user = _mapper.Map<User>(registerUser);
            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, ConstsRoles.User);



            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/Account/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            //var message = new MailMessage(new string[] { user.Email }, "Confirmation email link", confirmationLink);
            //_mailingService.SendMail(message);
            return result;
        }
        #endregion
        #region methods
        public async Task<bool> UpdateUserInfo(UserDto userDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                return false;
            try
            {
                user = _mapper.Map(userDto, user);
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
