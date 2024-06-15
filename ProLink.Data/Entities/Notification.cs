namespace ProLink.Data.Entities
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public string? AboutUserId { get; set; }

        public NotificationType Type { get; set; }
    }

    public enum NotificationType
    {
        React,
        Comment,
        Follow,
        Mention,
        Message,
        Post,
        Job,
        CompleteJob,
        JobRequest,
        FriendRequest
    }
}
