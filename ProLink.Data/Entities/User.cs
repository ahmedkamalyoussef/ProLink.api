using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProLink.Data.Entities
{
    public class User : IdentityUser
    {
        public string? OTP { get; set; }
        public DateTime OTPExpiry { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? JopTitle { get; set; }
        public string? BackImage {  get; set; }
        public string? Description { get; set; }
        public string? FriendId { get; set; }
        public string? FollowerId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CV { get; set; }
        [InverseProperty("Sender")]
        public virtual ICollection<JobRequest>? SentJobRequests { get; set; }
        [InverseProperty("Receiver")]
        public virtual ICollection<JobRequest>? ReceivedJobRequests { get; set; }
        public virtual ICollection<FriendRequest>? SentFriendRequests { get; set; }
        public virtual ICollection<FriendRequest>? ReceivedFriendRequests { get; set; }
        public virtual List<string>? Skills { get; set; }
        public virtual ICollection<Job>? CompletedJobs { get; set; }
        public virtual ICollection<Job>? Jobs { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Post>? LikedPosts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<React>? Reacts { get; set; }
        public virtual ICollection<Message>? SentMessages { get; set; }
        public virtual ICollection<Message>? ReceivedMessages { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<UserFollower>? Followers { get; set; }
        public virtual ICollection<UserFriend>? Friends { get; set; }

    }
}
