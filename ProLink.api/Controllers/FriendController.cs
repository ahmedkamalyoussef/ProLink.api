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
        private readonly IFriendService _friendService;
        #endregion

        #region ctor
        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }
        #endregion

        #region actions
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetFriendsAsync()
        {
            var result = await _friendService.GetFriendsAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFriendAsync(string friendId)
        {
            var result = await _friendService.DeleteFriendAsync(friendId);
            return result ? Ok("friend has been deleted successfully") : BadRequest("faild to delete friend");
        }
        #endregion
    }
}
