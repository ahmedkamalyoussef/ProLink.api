using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class UserFollower
    {
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public string FollowerId { get; set; }

        public virtual User Follower { get; set; }
    }
}
