using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Entities
{
    public class JobRequest
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }= Guid.NewGuid().ToString();  
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual User Creator { get; set; }
        public virtual ICollection<User> Recipients { get; set; }
    }

    

}
