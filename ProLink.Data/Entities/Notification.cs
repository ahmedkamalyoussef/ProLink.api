using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Notification
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
