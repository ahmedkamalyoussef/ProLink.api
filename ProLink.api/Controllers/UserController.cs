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
                return result != null ? Ok(result) : BadRequest("Failed to update user information.");
        }

        [Authorize]
        [HttpGet("get-user-by-id")]
        public async Task<IActionResult> GetUserByIDAsync(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return result != null ? Ok(result) : BadRequest("Failed to update user information.");
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
                return success? Ok():NotFound();
        }
        #endregion

        #region file handling
        [Authorize]
        [HttpGet("get-user-picture")]
        public async Task<IActionResult> GetUserPictureAsync()
        {
            var result = await _userService.GetUserPictureAsync();
            return result!=string.Empty? Ok(result):BadRequest("there is not picture.");
        }
        [Authorize]
        [HttpPost("add-user-picture")]
        public async Task<IActionResult> AddUserPictureAsync(IFormFile file)
        {
            var result =await _userService.AddUserPictureAsync(file);
            return result?Ok("picture has been added successfully."):BadRequest("failed to add picture");
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
        #endregion

        #region skill actions
        [Authorize]
        [HttpPost("add-skill")]
        public async Task<IActionResult> AddSkillAsync(AddSkillDto addSkilltDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.AddSkillAsync(addSkilltDto);
            return result ? Ok("Skill has been added successfully") : BadRequest("faild to add Skill");
        }

        [Authorize]
        [HttpGet("get-user-skills")]
        public async Task<IActionResult> GetCurrentUserSkillsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.GetCurrentUserSkillsAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-user-skills-by-Id")]
        public async Task<IActionResult> GetUserSkillsByIdAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.GetUserSkillsByIdAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update-skill")]
        public async Task<IActionResult> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateSkillAsync(skillId, addSkillDto);
            return result ? Ok("Skill has been updated successfully") : BadRequest("faild to update Skill");
        }
        [Authorize]
        [HttpDelete("delete-Skill")]
        public async Task<IActionResult> DeleteSkillAsync(string skillId)
        {
            var result = await _userService.DeleteSkillAsync(skillId);
            return result ? Ok("Skill has been deleted successfully") : BadRequest("faild to delete Skill");
        }
        #endregion
    }
}
