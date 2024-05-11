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
        private readonly ISkillService _skillService;
        #endregion

        #region ctor
        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }
        #endregion

        #region skill actions
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddSkillAsync(AddSkillDto addSkilltDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _skillService.AddSkillAsync(addSkilltDto);
            return result ? Ok("Skill has been added successfully") : BadRequest("faild to add Skill");
        }

        [Authorize]
        [HttpGet("get-user-skills")]
        public async Task<IActionResult> GetCurrentUserSkillsAsync()
        {
            var result = await _skillService.GetCurrentUserSkillsAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("get-user-skills-by-Id")]
        public async Task<IActionResult> GetUserSkillsByIdAsync(string id)
        {
            var result = await _skillService.GetUserSkillsByIdAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _skillService.UpdateSkillAsync(skillId, addSkillDto);
            return result ? Ok("Skill has been updated successfully") : BadRequest("faild to update Skill");
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSkillAsync(string skillId)
        {
            var result = await _skillService.DeleteSkillAsync(skillId);
            return result ? Ok("Skill has been deleted successfully") : BadRequest("faild to delete Skill");
        }
        #endregion
    }
}
