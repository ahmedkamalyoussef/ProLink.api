using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public SkillController(IUserService userService)
        {
            _userService = userService;
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
            var result = await _userService.GetCurrentUserSkillsAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-user-skills-by-Id")]
        public async Task<IActionResult> GetUserSkillsByIdAsync(string id)
        {
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
