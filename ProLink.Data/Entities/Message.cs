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
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
    }
}
