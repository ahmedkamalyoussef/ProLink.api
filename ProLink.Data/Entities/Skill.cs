using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Entities
{
    public class Skill
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SkillId { get; set; }=Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? Description { get; set; }

        //public virtual ICollection<UserSkill>? UserSkills { get; set; }
    }

}
