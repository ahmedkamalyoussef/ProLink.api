using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface INotificationService
    {
        
        Task<List<NotificationResultDto>> GetCurrentUserNotificationsAsync();
        Task<NotificationResultDto> GetNotificationByIdAsync(string notificationId);
        Task<bool> DeleteAllNotificationAsync();
        Task<bool> DeleteNotificationByIdAsync(string notificationId);
    }
}
