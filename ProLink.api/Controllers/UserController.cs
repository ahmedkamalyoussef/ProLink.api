using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region user actions
        [Authorize]
        [HttpGet("get-Current-user")]
        public async Task<IActionResult> GetCurrentUserInfoAsync()
        {
            var result = await _userService.GetCurrentUserInfoAsync();
                return result != null ? Ok(result) : BadRequest(" user not found.");
        }

        [Authorize]
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserByIDAsync(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result != null ? Ok(result) : BadRequest("user not found.");
        }
        [Authorize]
        [HttpGet("get-user-by-name")]
        public async Task<IActionResult> GetUserNameAsync(string name)
        {
            var result = await _userService.GetUsersByNameAsync(name);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("update-user-info")]
        public async Task<IActionResult> UpdateUserInfoAsync(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _userService.UpdateUserInfoAsync(userDto);
                return success?Ok("user information updated successfully."):
                    BadRequest("Failed to update user information.");
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var success = await _userService.DeleteAccountAsync();
                return success? Ok():BadRequest("faild to delete user");
        }
        #endregion

        #region file handling
        [Authorize]
        [HttpGet("get-user-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _userService.GetUserPictureAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Update-user-picture")]
        public async Task<IActionResult> UpdateUserPictureAsync(IFormFile file)
        {
            var result = await _userService.UpdateUserPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [Authorize]
        [HttpDelete("delete-user-picture")]
        public async Task<IActionResult> DeleteUserPictureAsync()
        {
            var result = await _userService.DeleteUserPictureAsync();
            return result ? Ok("picture has been deleted successfully.") : BadRequest("failed to delete picture");
        }



        [Authorize]
        [HttpGet("get-user-CV")]
        public async Task<IActionResult> GetUserCVAsync()
        {
            var result = await _userService.GetUserCVAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Update-user-CV")]
        public async Task<IActionResult> UpdateUserCVAsync(IFormFile file)
        {
            var result = await _userService.UpdateUserCVAsync(file);
            return result ? Ok("CV has been added successfully.") : BadRequest("failed to add CV");
        }
        [Authorize]
        [HttpDelete("delete-user-CV")]
        public async Task<IActionResult> DeleteUserCVAsync()
        {
            var result = await _userService.DeleteUserCVAsync();
            return result ? Ok("CV has been deleted successfully.") : BadRequest("failed to delete CV");
        }



        [Authorize]
        [HttpGet("get-user-BackImage")]
        public async Task<IActionResult> GetUserBackImageAsync()
        {
            var result = await _userService.GetUserBackImageAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Update-user-BackImage")]
        public async Task<IActionResult> UpdateUserBackImageAsync(IFormFile file)
        {
            var result = await _userService.UpdateUserBackImageAsync(file);
            return result ? Ok("BackImage has been added successfully.") : BadRequest("failed to add BackImage");
        }
        [Authorize]
        [HttpDelete("delete-user-BackImage")]
        public async Task<IActionResult> DeleteUserBackImageAsync()
        {
            var result = await _userService.DeleteUserBackImageAsync();
            return result ? Ok("BackImage has been deleted successfully.") : BadRequest("failed to delete BackImage");
        }
        #endregion
 
    }
}
