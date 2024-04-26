using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class FriendRequest
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateSent { get; set; }
        public FriendRequestStatus Status { get; set; }

        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        [InverseProperty("SentFriendRequests")]
        public virtual User Sender { get; set; }

        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceivedFriendRequests")]
        public virtual User Receiver { get; set; }
    }

}
