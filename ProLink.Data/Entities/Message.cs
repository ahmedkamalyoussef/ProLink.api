using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Entities
{
    public class Message
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        [InverseProperty("SentMessages")]
        public virtual User Sender { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        [InverseProperty("ReceivedMessages")]
        public virtual User Receiver { get; set; }
    }
}
