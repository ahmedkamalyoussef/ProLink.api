using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Skill
    {
        [Key]
        public string SkillId { get; set; }=Guid.NewGuid().ToString();
        public string Name { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

}
