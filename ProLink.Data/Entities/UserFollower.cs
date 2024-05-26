using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class UserFollower
    {
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string FollowerId { get; set; }
        [Required]
        [ForeignKey(nameof(FollowerId))]
        public virtual User Follower { get; set; }
    }
}
