using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Application.Authentication;
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
