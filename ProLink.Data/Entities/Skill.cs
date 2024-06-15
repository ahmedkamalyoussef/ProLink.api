using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Skill
    {
        public string SkillId { get; set; }=Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }

}
