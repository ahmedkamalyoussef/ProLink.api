using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        #region fields
        private readonly IFollowerService _friendService;
        #endregion

        #region ctor
        public FollowerController(IFollowerService friendService)
        {
            _friendService = friendService;
        }
        #endregion

        #region actions
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetFriendsAsync()
        {
            var result = await _friendService.GetFollowesAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("follow")]
        public async Task<IActionResult> FollowAsync(string userId)
        {
            var result = await _friendService.FollowAsync(userId);
            return result ? Ok("followed successfully") : BadRequest("faild to follow");
        }

        [Authorize]
        [HttpPut("unfollow")]
        public async Task<IActionResult> UnFollowAsync(string userId)
        {
            var result = await _friendService.UnFollowAsync(userId);
            return result ? Ok("Unfollowed successfully") : BadRequest("faild to Unfollow");
        }
        #endregion
    }
}
