using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public NotificationController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Notification
        [Authorize]
        [HttpGet("get-all-notifications")]
        public async Task<IActionResult> GetCurrentUserNotificationsAsync()
        {
            var result = await _userService.GetCurrentUserNotificationsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-notification-by-id")]
        public async Task<IActionResult> GetNotificationByIdAsync(string notificationId)
        {
            var result = await _userService.GetNotificationByIdAsync(notificationId);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("delete-notification-by-id")]
        public async Task<IActionResult> DeleteNotificationByIdAsync(string notificationId)
        {
            var result = await _userService.DeleteNotificationByIdAsync(notificationId);
            return result ? Ok("notification has been deleted successfully.") : BadRequest("notification to delete message");
        }
        [Authorize]
        [HttpDelete("delete-all-notifications")]
        public async Task<IActionResult> DeleteAllNotificationAsync()
        {
            var result = await _userService.DeleteAllNotificationAsync();
            return result ? Ok("notifications have been deleted successfully.") : BadRequest("failed to delete notifications");
        }
        #endregion
    }
}
