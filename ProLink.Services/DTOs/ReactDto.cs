using ProLink.Data.Consts;

namespace ProLink.Application.DTOs
{
    public class ReactDto
    {
        public string Id { get; set; } 
        public DateTime DateReacted { get; set; }
        public ReactType Type { get; set; }
        public UserResultDto User { get; set; }
    }
}
