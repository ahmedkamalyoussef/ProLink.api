using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Entities
{
    public class UserFriend
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        //[InverseProperty("Receiver")]
        public string FriendId { get; set; }
        [ForeignKey("FriendId")]
        //[InverseProperty("Receiver")]
        public virtual User Friend { get; set; }

    }
}
