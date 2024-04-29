using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Application.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion
        #region ctor
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region registration
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RegisterAsync(user);
            if (result.Succeeded)
            {
                return Ok("Registration succeeded.");
            }
            return BadRequest(result.Errors);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string email, [FromQuery] string token)
        {
            var success = await _userService.ConfirmEmailAsync(email, token);
            if (success)
                return Ok("Email confirmed successfully.");

            return BadRequest("Failed to confirm email.");
        }
        #endregion

        #region login & logout
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginUser loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(loginUser);
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
            else
            {
                errorMessage = "Invalid login attempt.";
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return Unauthorized(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _userService.LogoutAsync();

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            else
            {
                return BadRequest(new { error = result.Message });
            }
        }
        #endregion

        #region forget & reset & change password
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isSent = await _userService.ForgetPasswordAsync(email);
            if (isSent)
            {
                return Ok("Reset password email sent successfully.");
            }
            else
            {
                return NotFound("User with provided email not found.");
            }
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };

            return Ok(new {model});
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resetPasswordResult = await _userService.ResetPasswordAsync(resetPassword);
            if (!resetPasswordResult.Succeeded)
            {
                foreach (var error in resetPasswordResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            return Ok("Password reset successfully");

        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var changePasswordResult = await _userService.ChangePasswordAsync(changePassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok("Password changed successfully");
        }
        #endregion


        [Authorize]
        [HttpGet("get-Current-user")]
        public async Task<IActionResult> GetCurrentUserInfoAsync()
        {
            var result = await _userService.GetCurrentUserInfoAsync();

            if (result!=null)
            {
                return Ok(result);
            }
            return BadRequest("Failed to update user information.");

        }


        [Authorize]
        [HttpPut("update-user-info")]
        public async Task<IActionResult> UpdateUserInfoAsync(UserDto userDto)
        {
            var success = await _userService.UpdateUserInfoAsync(userDto);

            if (!success)
            {
                return BadRequest("Failed to update user information.");
            }

            return Ok("user information updated successfully.");
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var success = await _userService.DeleteAccountAsync();

            if (!success)
            {
                return NotFound();
            }

            return Ok();
        }

        #region file handling
        [HttpGet("get-user-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _userService.GetUserPictureAsync();
            return result!=string.Empty? Ok(result):BadRequest("there is not picture.");
        }

        [HttpPost("add-user-picture")]
        public async Task<IActionResult> AddUserPictureAsync(IFormFile? file)
        {
            var result =await _userService.AddUserPictureAsync(file);
            return result?Ok("picture has been added successfully."):BadRequest("failed to add picture");
        }
        [HttpPut("Update-user-picture")]
        public async Task<IActionResult> UpdateUserPictureAsync(IFormFile? file)
        {
            var result = await _userService.UpdateUserPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [HttpDelete("delete-user-picture")]
        public async Task<IActionResult> DeleteUserPictureAsync()
        {
            var result = await _userService.DeleteUserPictureAsync();
            return result ? Ok("picture has been deleted successfully.") : BadRequest("failed to delete picture");
        }
        #endregion
    }
}
