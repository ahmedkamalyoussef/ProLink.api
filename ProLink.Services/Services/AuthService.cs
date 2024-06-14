using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Mail;
using ProLink.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ProLink.Application.Services
{
    public class AuthService : IAuthService
    {
        #region fields
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;
        private SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region ctor
        public AuthService(
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers,
            IMailingService mailingService,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Registration
        public async Task<IdentityResult> RegisterAsync(RegisterUser registerUser)
        {
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);

            if (userExist != null)
            {
                if(userExist.EmailConfirmed)
                {
                    await SendOTPAsync(userExist.Email);
                    return IdentityResult.Success;
                }
                throw new Exception("user already exist");
                
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
        public async Task<AuthDTO> LoginAsync(LoginUser loginUser)
        {
            var authModel = new AuthDTO();
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user == null)
                    return new AuthDTO { Message = "user not found" };

                if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
                    return new AuthDTO { Message = "Invalid password" };
                if (!user.EmailConfirmed)
                {
                    return new AuthDTO { Message = "user not confirmed" };
                }

                var token = await _userHelpers.GenerateJwtTokenAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                authModel.Message = $"Welcome Back, {user.FirstName}";
                authModel.UserName = user.UserName;
                authModel.Email = user.Email;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
                authModel.IsAuthenticated = true; //ExpiresOn = token.ValidTo,
                authModel.Roles = roles.ToList();


                if (user.RefreshTokens.Any(a => a.IsActive))
                {
                    var ActiveRefreshToken = user.RefreshTokens.First(a => a.IsActive);
                    authModel.RefreshToken = ActiveRefreshToken.Token;
                    authModel.RefreshTokenExpiration = ActiveRefreshToken.ExpiresOn;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);
                    authModel.RefreshToken = refreshToken.Token;
                    authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                }
                //var message = new MailMessage(new[] { user.Email }, "Login", "You logged in to your account right now.");
                //_mailingService.SendMail(message);
                return authModel;
            }
            catch (Exception ex)
            {
                return new AuthDTO { Message = "Invalid Authentication", Errors = new List<string> { ex.Message } };
            }
        }

        #endregion
        #region logout
        public async Task<string> LogoutAsync()
        {
            if (await _userHelpers.GetCurrentUserAsync() == null)
            {
                return "User Not Found";
            }
            await _signInManager.SignOutAsync();

            return "User Logged Out Successfully";
        }
        #endregion
        #region Refresh Token
        public async Task<AuthDTO> RefreshTokenAsync(string Token)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
            if (user == null)
            {
                return new AuthDTO { Message = "Invalid Token" };
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
            if (!refreshToken.IsActive)
            {
                return new AuthDTO { Message = "InActive Token" };
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtToken = await _userHelpers.GenerateJwtTokenAsync(user);
            return new AuthDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                IsAuthenticated = true,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user) as List<string>,
            };
        }
        #endregion

        #region Revoke Token
        public async Task<bool> RevokeTokenAsync(string Token)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
            if (user == null)
            {
                return false;
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
            if (!refreshToken.IsActive)
            {
                return false;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
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

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddMinutes(15),
                CreatedOn = DateTime.UtcNow,
            };
        }
        #endregion
    }
}
