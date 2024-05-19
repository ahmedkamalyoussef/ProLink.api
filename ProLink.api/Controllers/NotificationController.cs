using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;
using ProLink.Application.Services;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region fields
        private readonly INotificationService _notificationService;
        #endregion

        #region ctor
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        #endregion

        #region Notification
        [Authorize]
        [HttpGet("get-user-all")]
        public async Task<IActionResult> GetCurrentUserNotificationsAsync()
        {
            var result = await _notificationService.GetCurrentUserNotificationsAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNotificationByIdAsync(string notificationId)
        {
            var result = await _notificationService.GetNotificationByIdAsync(notificationId);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteNotificationByIdAsync(string notificationId)
        {
            var result = await _notificationService.DeleteNotificationByIdAsync(notificationId);
            return result ? Ok("notification has been deleted successfully.") : BadRequest("notification to delete message");
        }
        [Authorize]
        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllNotificationAsync()
        {
            var result = await _notificationService.DeleteAllNotificationAsync();
            return result ? Ok("notifications have been deleted successfully.") : BadRequest("failed to delete notifications");
        }
        #endregion
    }
}
