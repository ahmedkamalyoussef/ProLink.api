using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ProLink.Application.Authentication;
using ProLink.Application.Consts;
using ProLink.Application.Mail;
using MailMessage = ProLink.Application.Mail.MailMessage;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMailingService _mailingService;
        #endregion

        #region ctor
        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUserHelpers userHelpers,
            SignInManager<User> signInManager,
            IMailingService mailingService
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _signInManager = signInManager;
            _mailingService = mailingService;
        }
        #endregion
        #region Registration
        public async Task<IdentityResult> RegisterAsync(RegisterUser registerUser)
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
            var message = new MailMessage(new string[] { user.Email }, "Confirmation email link", confirmationLink);
            _mailingService.SendMail(message);
            return result;
        }


        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        #endregion

        #region login & logout
        public async Task<LoginResult> LoginAsync(LoginUser loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null)
            {   
                return new LoginResult
                {
                    Success = false,
                    Token = null,
                    Expiration = default,
                    ErrorType = LoginErrorType.UserNotFound
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                return new LoginResult
                {
                    Success = false,
                    Token = null,
                    Expiration = default,
                    ErrorType = LoginErrorType.InvalidPassword
                };
            }

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

            return await _userHelpers.GenerateJwtTokenAsync(claims);
        }

        public async Task<LogoutResult> LogoutAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new LogoutResult
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }
            try
            {
                await _signInManager.SignOutAsync();
                return new LogoutResult
                {
                    Success = true,
                    Message = "User successfully logged out."
                };
            }
            catch
            {
                return new LogoutResult
                {
                    Success = false,
                    Message = "An error occurred while logging out."
                };
            }
        }
        #endregion

        #region forget & reset & change password
        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/api/Account/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";
            var message = new MailMessage(new string[] { user.Email }, "reset email link", resetLink);
            _mailingService.SendMail(message);
            return true;
        }


        public async Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword)
        {

            var customer = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (customer == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });


            var result = await _userManager.ResetPasswordAsync(customer, resetPassword.Token, resetPassword.NewPassword);
            return result;

        }


        public async Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            return result;
        }
        #endregion

        #region methods

        public async Task<UserDto> GetCurrentUserInfoAsync()
        {
            var currentUser= await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("User not found.");
            var user =_mapper.Map<UserDto>(currentUser);
            return user;
        }

        public async Task<bool> UpdateUserInfoAsync(UserDto userDto)
        {   
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new ArgumentNullException("user not found");
            try
            {
                currentUser = _mapper.Map(userDto,currentUser);
                _unitOfWork.User.Update(currentUser);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAccountAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }


        #endregion
        #region file handlling
        public async Task<bool> AddUserPictureAsync(IFormFile file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var picture = await _userHelpers.AddImageAsync(file);
            if (picture != null)
                user.ProfilePicture = picture;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteImageAsync(oldPicture);
            return false;
        }

        public async Task<bool> UpdateUserPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newPicture = await _userHelpers.AddImageAsync(file);
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = newPicture;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0 && !oldPicture.IsNullOrEmpty())
            {
                return await _userHelpers.DeleteImageAsync(oldPicture);
            }
             await _userHelpers.DeleteImageAsync(newPicture);
            return false;
        }

        public async Task<string> GetUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.ProfilePicture.IsNullOrEmpty())
                throw new Exception("User dont have profile picture");
            return user.ProfilePicture;
        }
        #endregion
    }
}
