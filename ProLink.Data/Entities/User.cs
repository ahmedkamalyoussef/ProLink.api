using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? JopTitle { get; set; }

        public string? Description { get; set; }

        public string? ProfilePicture { get; set; }
        public virtual ICollection<Rate>? Rates { get; set; }
        public virtual ICollection<User>? Friends { get; set; }
        public virtual ICollection<Skill>? Skills { get; set; }
    }
}
