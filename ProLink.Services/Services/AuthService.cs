using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.Helpers;
using ProLink.Application.Mail;
using ProLink.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProLink.Application.Services
{
    public class AuthService : IAuthService
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
            {
                await SendOTPAsync(userExist.Email);
                return IdentityResult.Success;
            }

            var user = _mapper.Map<User>(registerUser);
            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var otp = GenerateOTP();
                user.OTP = otp;
                user.OTPExpiry = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);

                var message = new MailMessage(new[] { user.Email }, "Your OTP for Email Confirmation", $"Your OTP is: {otp}");
                _mailingService.SendMail(message);
            }
            return result;
        }

        
        #endregion

        #region login
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
                    ErrorType = LoginErrorType.EmailNotConfirmed
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
            var message = new MailMessage(new[] { user.Email }, "Login", "You logged in to your account right now.");
            _mailingService.SendMail(message);
            return await _userHelpers.GenerateJwtTokenAsync(claims);
        }

        #endregion

        #region Password Management
        public async Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (user.OTP != changePassword.OTP || user.OTPExpiry < DateTime.UtcNow)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired OTP" });

            user.OTP = null;
            user.OTPExpiry = DateTime.MinValue;

            var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            await SendOTPAsync(email);
            return true;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (user.OTP != resetPassword.OTP || user.OTPExpiry < DateTime.UtcNow)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired OTP" });

            user.OTP = null;
            user.OTPExpiry = DateTime.MinValue;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
            await _userManager.UpdateAsync(user);

            return result;
        }
        #endregion

        #region OTP Management
        public async Task<IdentityResult> SendOTPAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var otp = GenerateOTP();
            user.OTP = otp;
            user.OTPExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            var message = new MailMessage(new[] { user.Email }, "Your OTP", $"Your OTP is: {otp}");
            _mailingService.SendMail(message);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> VerifyOTPAsync(VerifyOTPRequest verifyOTPRequest)
        {
            var user = await _userManager.FindByEmailAsync(verifyOTPRequest.Email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (user.OTP != verifyOTPRequest.OTP || user.OTPExpiry < DateTime.UtcNow)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired OTP" });

            user.OTP = string.Empty;
            user.OTPExpiry = DateTime.MinValue;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return IdentityResult.Success;
        }
        #endregion

        #region private methods
        private string GenerateOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[6];
                rng.GetBytes(byteArray);

                var sb = new StringBuilder();
                foreach (var byteValue in byteArray)
                {
                    sb.Append(byteValue % 10);
                }
                return sb.ToString();
            }
        }
        #endregion
    }
}
