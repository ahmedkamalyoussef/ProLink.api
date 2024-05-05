using ProLink.Data.Consts;

namespace ProLink.Application.DTOs
{
    public class JobRequestDto
    {
        public string Id { get; set; } 
        public string? CV { get; set; }
        public Status? Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string SenderId { get; set; }
        public string PostId { get; set; }
    }
}
