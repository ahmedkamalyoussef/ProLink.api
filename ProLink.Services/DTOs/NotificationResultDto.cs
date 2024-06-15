using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class NotificationResultDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationType Type { get; set; }
        public UserPostResultDTO? Sender { get; set; }
    }
}
