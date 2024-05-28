using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class Post
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        [Required]
        public string Description { get; set; }
        public string? PostImage { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }

}
