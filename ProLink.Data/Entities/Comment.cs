using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; } =Guid.NewGuid().ToString();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
    }
}
