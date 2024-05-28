using ProLink.Data.Consts;
using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class JobResultDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsRequestSent { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public string? PostImage { get; set; }
        public Status Status { get; set; }
        public UserResultDto User { get; set; }

    }
}
