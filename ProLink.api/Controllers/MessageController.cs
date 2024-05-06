using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public MessageController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region messages
        [Authorize]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessageAsync(string recieverId, SendMessageDto sendMessageDto)
        {
            var result = await _userService.SendMessageAsync(recieverId, sendMessageDto);
            return result ? Ok("meassge has been sent successfully.") : BadRequest("failed to send message");
        }
        [Authorize]
        [HttpPut("update-message")]
        public async Task<IActionResult> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto)
        {
            var result = await _userService.UpdateMessageAsync(messageId, sendMessageDto);
            return result ? Ok("meassge has been updated successfully.") : BadRequest("failed to update message");
        }
        [Authorize]
        [HttpDelete("delete-message")]
        public async Task<IActionResult> DeleteMessageAsync(string messageId)
        {
            var result = await _userService.DeleteMessageAsync(messageId);
            return result ? Ok("meassge has been deleted successfully.") : BadRequest("failed to delete message");
        }
        #endregion
    }
}
