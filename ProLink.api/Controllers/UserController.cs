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
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfoToUpdateAsync()
        {
            var result = await _userService.GetUserInfoAsync();
            return result != null ? Ok(result) : BadRequest(" user not found.");
        }

        [Authorize]
        [HttpGet("by-id")]
        public async Task<IActionResult> GetUserByIDAsync(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result != null ? Ok(result) : BadRequest("user not found.");
        }
        [Authorize]
        [HttpGet("by-name-or-title")]
        public async Task<IActionResult> GetUserByNameOrTitleAsync(string name)
        {
            var result = await _userService.GetUsersByNameOrTitleAsync(name);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("info")]
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

        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var success = await _userService.DeleteAccountAsync();
                return success? Ok():BadRequest("faild to delete user");
        }
        #endregion

        #region file handling
        [Authorize]
        [HttpGet("get-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _userService.GetUserPictureAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Update-picture")]
        public async Task<IActionResult> UpdateUserPictureAsync(/*IFormFile*/string file)
        {
            var result = await _userService.UpdateUserPictureAsync(file);
            return result ? Ok("picture has been added successfully.") : BadRequest("failed to add picture");
        }
        [Authorize]
        [HttpDelete("delete-picture")]
        public async Task<IActionResult> DeleteUserPictureAsync()
        {
            var result = await _userService.DeleteUserPictureAsync();
            return result ? Ok("picture has been deleted successfully.") : BadRequest("failed to delete picture");

        }



        [Authorize]
        [HttpGet("get-CV")]
        public async Task<IActionResult> GetUserCVAsync()
        {
            var result = await _userService.GetUserCVAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Update-CV")]
        public async Task<IActionResult> UpdateUserCVAsync(/*IFormFile*/string file)
        {
            var result = await _userService.UpdateUserCVAsync(file);
            return result ? Ok("CV has been added successfully.") : BadRequest("failed to add CV");
        }
        [Authorize]
        [HttpDelete("delete-CV")]
        public async Task<IActionResult> DeleteUserCVAsync()
        {
            var result = await _userService.DeleteUserCVAsync();
            return result ? Ok("CV has been deleted successfully.") : BadRequest("failed to delete CV");
        }



        [Authorize]
        [HttpGet("get-BackImage")]
        public async Task<IActionResult> GetUserBackImageAsync()
        {
            var result = await _userService.GetUserBackImageAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPut("Update-BackImage")]
        public async Task<IActionResult> UpdateUserBackImageAsync(/*IFormFile*/string file)
        {
            var result = await _userService.UpdateUserBackImageAsync(file);
            return result ? Ok("BackImage has been added successfully.") : BadRequest("failed to add BackImage");
        }
        [Authorize]
        [HttpDelete("delete-BackImage")]
        public async Task<IActionResult> DeleteUserBackImageAsync()
        {
            var result = await _userService.DeleteUserBackImageAsync();
            return result ? Ok("BackImage has been deleted successfully.") : BadRequest("failed to delete BackImage");
        }
        #endregion
 
    }
}
