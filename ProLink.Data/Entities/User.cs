using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Data.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? JopTitle { get; set; }

        public string? Description { get; set; }

        public string? ProfilePicture { get; set; }

        public virtual ICollection<UserSkill>? UserSkills { get; set; }
        
        public virtual ICollection<FriendRequest>? SentFriendRequests { get; set; }
        
        public virtual ICollection<FriendRequest>? ReceivedFriendRequests { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<JobRequest>? SentJobRequests { get; set; }
        public virtual ICollection<JobRequest>? ReceivedJobRequests { get; set; }
        public virtual ICollection<Message>? SentMessages { get; set; }
        public virtual ICollection<Message>? ReceivedMessages { get; set; }
        //public virtual ICollection<UserFriend>? Friends { get; set; }
        public virtual ICollection<Post>? PostedJobs { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }
}
