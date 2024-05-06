using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public FriendController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region friend
        [Authorize]
        [HttpGet("Get-friends")]
        public async Task<IActionResult> GetFriendsAsync()
        {
            var result = await _userService.GetFriendsAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("delete-friend")]
        public async Task<IActionResult> DeleteFriendAsync(string friendId)
        {
            var result = await _userService.DeleteFriendAsync(friendId);
            return result ? Ok("friend has been deleted successfully") : BadRequest("faild to delete friendRequest");
        }
        #endregion
    }
}
