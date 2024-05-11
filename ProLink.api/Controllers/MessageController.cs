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
        private readonly IMessageService _messageService;
        #endregion

        #region ctor
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion

        #region messages
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetMessageAsync(string recieverId)
        {
            var result = await _messageService.GetMessagesAsync(recieverId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendMessageAsync(string recieverId, SendMessageDto sendMessageDto)
        {
            var result = await _messageService.SendMessageAsync(recieverId, sendMessageDto);
            return result ? Ok("meassge has been sent successfully.") : BadRequest("failed to send message");
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto)
        {
            var result = await _messageService.UpdateMessageAsync(messageId, sendMessageDto);
            return result ? Ok("meassge has been updated successfully.") : BadRequest("failed to update message");
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMessageAsync(string messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);
            return result ? Ok("meassge has been deleted successfully.") : BadRequest("failed to delete message");
        }
        #endregion
    }
}
