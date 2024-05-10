using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class JobRequest
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();  
        public string? CV { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }
        [Required]
        public string RecieverId { get; set; }
        [Required]
        [ForeignKey("RecieverId")]
        public virtual User Receiver { get; set; }
        [Required]
        public string PostId { get; set; }
        [Required]
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }

    

}
