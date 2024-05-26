using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class UserFriend
    {
        public  string FriendId { get; set; }
        [Required]
        [ForeignKey(nameof(FriendId))]
        public virtual User Friend { get; set; }
        public  string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
