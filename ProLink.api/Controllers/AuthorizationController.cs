using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Authentication;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        #region fields
        private readonly IAuthService _authService;
        #endregion

        #region ctor
        public AuthorizationController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var result = await _authService.RegisterAsync(registerUser);
            if (result.Succeeded)
            {
                return Ok("OTP sent to email");
            }

            return BadRequest(result.Errors);
        }

        
        #endregion

        #region login
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginUser loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(loginUser);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        #endregion

        #region LogOut
        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var result = await _authService.LogoutAsync();
            if (result == "User Logged Out Successfully")
                return Ok(result);
            return BadRequest(result);
        }
        #endregion

        #region Refresh Token
        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Invalid Token");
            }
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        #endregion

        #region Revoke Token
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeTokenAsync(string? Token)
        {
            Token = Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(Token))
            {
                return BadRequest("Token is Required");
            }
            var result = await _authService.RevokeTokenAsync(Token);
            if (result)
            {
                return Ok("Token Revoked Successfully");
            }
            return BadRequest("Token Not Revoked");
        }
        #endregion

        #region Set Refresh Token In Cookie
        private void SetRefreshTokenInCookie(string Token, DateTime expires)
        {
            var CoockieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
            };
            Response.Cookies.Append("refreshToken", Token, CoockieOptions);
        }
        #endregion

        #region forget & reset & change password

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var result = await _authService.ChangePasswordAsync(changePassword);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _authService.ForgetPasswordAsync(email);
            if (result)
                return Ok();

            return BadRequest("User not found");
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var result = await _authService.ResetPasswordAsync(resetPassword);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }
        #endregion

        #region OTP management
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(string email)
        {
            var result = await _authService.SendOTPAsync(email);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPRequest request)
        {
            var result = await _authService.VerifyOTPAsync(request);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully");
            }
            return BadRequest(result.Errors);
        }
        #endregion
    }
}
