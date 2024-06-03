namespace ProLink.Application.DTOs
{
    public class UserPostResultDTO
    {
        public string Id { get; set; }
        public bool IsLiked { get; set; }
        public bool IsUserFollowed { get; set; }
        public string Description { get; set; }
        public string LikeId { get; set; }
        public string? PostImage { get; set; }
        public int CommentsCount { get; set; }
        public int ReactsCount { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<ReactDto> Reacts { get; set; } = new List<ReactDto>();
        public UserResultDto User { get; set; }
    }
}
