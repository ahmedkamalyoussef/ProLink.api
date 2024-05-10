using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class Message
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
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
