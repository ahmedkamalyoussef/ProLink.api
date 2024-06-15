using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class JobRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? CV { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public string RecieverId { get; set; }

        public virtual User Receiver { get; set; }

        public string JobId { get; set; }

        public virtual Job Job { get; set; }
    }

    

}
