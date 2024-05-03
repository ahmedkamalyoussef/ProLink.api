using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class LikeDto
    {
        public string Id { get; set; } 
        public DateTime DateLiked { get; set; }
        public UserResultDto User { get; set; }
    }
}
