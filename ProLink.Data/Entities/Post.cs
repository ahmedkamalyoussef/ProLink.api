using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Post
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? PostImage { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

}
