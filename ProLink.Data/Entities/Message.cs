using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class Message
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }
        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }
    }
}
