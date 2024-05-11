using ProLink.Data.Entities;


namespace ProLink.Application.DTOs
{
    public class MessageResultDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

    }
}
