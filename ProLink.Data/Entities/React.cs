using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProLink.Data.Consts;

namespace ProLink.Data.Entities
{
    public class React
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public DateTime DateReacted { get; set; }
        public ReactType Type { get; set; }
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
