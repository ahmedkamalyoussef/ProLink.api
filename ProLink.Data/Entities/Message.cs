using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Message
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        //public string SenderId { get; set; }
        //[ForeignKey("SenderId")]
        //[InverseProperty("SentMessages")]
        //public virtual User Sender { get; set; }
        //public string ReceiverId { get; set; }
        //[ForeignKey("ReceiverId")]
        //[InverseProperty("ReceivedMessages")]
        //public virtual User Receiver { get; set; }
    }
}
