using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.DTOs
{
    public class PostResultDto
    {
        public string Id { get; set; }
        public bool IsLiked { get; set; }
        public bool IsUserFollowed { get; set; }
        public string Description { get; set; }
        public string LikeId { get; set; }
        public string? PostImage { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<LikeDto> Likes { get; set; } = new List<LikeDto>();
        public UserResultDto User { get; set; }
    }
}
