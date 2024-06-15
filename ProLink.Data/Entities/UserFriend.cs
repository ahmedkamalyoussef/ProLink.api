using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class UserFriend
    {
        public  string FriendId { get; set; }

        public virtual User Friend { get; set; }
        public  string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
