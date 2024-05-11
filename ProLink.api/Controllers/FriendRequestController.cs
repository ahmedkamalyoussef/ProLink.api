using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        #region fields
        private readonly IFriendRequestService _friendRequestService;
        #endregion

        #region ctor
        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }
        #endregion

        #region friend Request actions

        [Authorize]
        [HttpGet("Get-friendRequests")]
        public async Task<IActionResult> GetFriendRequestAsync()
        {
            var result = await _friendRequestService.GetFriendRequistsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPost("send-friendRequest")]
        public async Task<IActionResult> SendFriendRequestAsync(string userId)
        {
            var result = await _friendRequestService.SendFriendAsync(userId);
            return result ? Ok("friendRequest has been sent successfully") : BadRequest("faild to send friendRequest");
        }

        [Authorize]
        [HttpPut("accept-all-friendRequests")]
        public async Task<IActionResult> AcceptAllFriendsAsync()
        {
            var result = await _friendRequestService.AcceptAllFriendsAsync();
            return result ? Ok("friend Requests have been accepted successfully") : BadRequest("faild to accept friend Requests");
        }
        [Authorize]
        [HttpPut("accept-friendRequest")]
        public async Task<IActionResult> AcceptFriendAsync(string friendRequestId)
        {
            var result = await _friendRequestService.AcceptFriendAsync(friendRequestId);
            return result ? Ok("friend Request has been accepted successfully") : BadRequest("faild to accept friend Request");
        }
        [Authorize]
        [HttpDelete("delete-friendRequest")]
        public async Task<IActionResult> DeleteFriendRequestAsync(string friendId)
        {
            var result = await _friendRequestService.DeletePendingFriendAsync(friendId);
            return result ? Ok("friendRequest has been deleted successfully") : BadRequest("faild to delete friendRequest");
        }

        [Authorize]
        [HttpPut("decline-friendRequest")]
        public async Task<IActionResult> DeclinePendingFriendAsync(string friendId)
        {
            var result = await _friendRequestService.DeclinePendingFriendAsync(friendId);
            return result ? Ok("friendRequest has been Declined successfully") : BadRequest("faild to Declined friendRequest");
        }
        #endregion
    }
}
