using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class Notification
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

    }
}
