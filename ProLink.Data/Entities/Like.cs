using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Like
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateLiked { get; set; }
        
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        [Required]
        public string PostId { get; set; }
        [Required]
        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }
    }

}
