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
        //[Authorize]
        //[HttpPost("add-user-picture")]
        //public async Task<IActionResult> AddUserPictureAsync(IFormFile file)
        //{
        //    var result =await _userService.AddUserPictureAsync(file);
        //    return result?Ok("picture has been added successfully."):BadRequest("failed to add picture");
        //}
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


        //[Authorize]
        //[HttpPost("add-user-CV")]
        //public async Task<IActionResult> AddUserCVAsync(IFormFile file)
        //{
        //    var result = await _userService.AddUserCVAsync(file);
        //    return result ? Ok("CV has been added successfully.") : BadRequest("failed to add CV");
        //}
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

        #region rate actions
        [Authorize]
        [HttpPost("add-rate")]
        public async Task<IActionResult> AddRateAsync(string userId,RateDto rateDto)
        {
            var result = await _userService.AddRateAsync(userId, rateDto);
            return result ? Ok("rate has been added successfully") : BadRequest("faild to add rate");
        }

        [Authorize]
        [HttpDelete("delete-rate")]
        public async Task<IActionResult> DeleteRateAsync(string rateId)
        {
            var result = await _userService.DeleteRateAsync(rateId);
            return result ? Ok("rate has been deleted successfully") : BadRequest("faild to delete rate");
        }
        #endregion

        #region friendRequest actions

        [Authorize]
        [HttpGet("Get-friendRequests")]
        public async Task<IActionResult> GetFriendRequestAsync()
        {
            var result = await _userService.GetFriendRequistsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPost("add-friendRequest")]
        public async Task<IActionResult> SendFriendRequestAsync(string userId)
        {
            var result = await _userService.SendFriendAsync(userId);
            return result ? Ok("friendRequest has been sent successfully") : BadRequest("faild to send friendRequest");
        }

        [Authorize]
        [HttpDelete("delete-friendRequest")]
        public async Task<IActionResult> DeleteFriendRequestAsync(string friendId)
        {
            var result = await _userService.DeletePendingFriendAsync(friendId);
            return result ? Ok("friendRequest has been deleted successfully") : BadRequest("faild to delete friendRequest");
        }

        [Authorize]
        [HttpPut("decline-friendRequest")]
        public async Task<IActionResult> DeclinePendingFriendAsync(string friendId)
        {
            var result = await _userService.DeclinePendingFriendAsync(friendId);
            return result ? Ok("friendRequest has been Declined successfully") : BadRequest("faild to Declined friendRequest");
        }
        #endregion

        #region jobRequest actions

        [Authorize]
        [HttpGet("Get-jobRequests")]
        public async Task<IActionResult> GetjobRequestAsync()
        {
            var result = await _userService.GetJobRequistAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPost("add-jobRequest")]
        public async Task<IActionResult> SendjobRequestAsync(string userId, string postId)
        {
            var result = await _userService.SendJobRequistAsync(userId, postId);
            return result ? Ok("jobRequest has been sent successfully") : BadRequest("faild to send jobRequest");
        }

        [Authorize]
        [HttpDelete("delete-jobRequest")]
        public async Task<IActionResult> DeletejobRequestAsync(string rateId)
        {
            var result = await _userService.DeletePendingJobRequestAsync(rateId);
            return result ? Ok("jobRequest has been deleted successfully") : BadRequest("faild to delete jobRequest");
        }

        [Authorize]
        [HttpPut("decline-jobRequest")]
        public async Task<IActionResult> DeclinejobRequestAsync(string rateId)
        {
            var result = await _userService.DeclinePendingJobRequestAsync(rateId);
            return result ? Ok("jobRequest has been Declined successfully") : BadRequest("faild to Declined jobRequest");
        }
        #endregion
    }
}
