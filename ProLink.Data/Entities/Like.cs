using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Like
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateLiked { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
    }

}
