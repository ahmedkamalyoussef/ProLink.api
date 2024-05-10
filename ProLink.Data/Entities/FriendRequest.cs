using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class FriendRequest
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateSent { get; set; }
        public Status Status { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
    }

}
