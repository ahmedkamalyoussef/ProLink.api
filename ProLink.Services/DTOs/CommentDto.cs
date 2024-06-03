
using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; } 
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public UserPostResultDTO User { get; set; }
    }
}
