using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Authentication;
using ProLink.Application.Interfaces;

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
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginUser);
            if (result.Success)
                return Ok(result);

            string errorMessage;
            if (result.ErrorType == LoginErrorType.UserNotFound)
            {
                errorMessage = "User not found.";
            }
            else if (result.ErrorType == LoginErrorType.InvalidPassword)
            {
                errorMessage = "Incorrect password.";
            }
            else if (result.ErrorType == LoginErrorType.EmailNotConfirmed)
            {
                errorMessage = "Email Not Comfirmed.";
            }
            else
            {
                errorMessage = "Invalid login attempt.";
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return Unauthorized(ModelState);
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
