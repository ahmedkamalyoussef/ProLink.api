using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.Consts;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Application.Mail;
using ProLink.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProLink.Application.Services
{
    public class AuthService:IAuthService
    {
        #region fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailingService _mailingService;
        #endregion

        #region ctor
        public AuthService(
            UserManager<User> userManager, IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUserHelpers userHelpers,
            SignInManager<User> signInManager,
            IMailingService mailingService
            )
        {
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
            if (!user.EmailConfirmed)
            {
                return new LoginResult
                {
                    Success = false,
                    Token = null,
                    Expiration = default,
                    ErrorType = LoginErrorType.EmailNotComfirmed
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
            var message = new MailMessage(new string[] { user.Email }, "login","you logged in your account right now");
            _mailingService.SendMail(message);
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
    }
}
