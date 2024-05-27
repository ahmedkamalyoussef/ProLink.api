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
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? PostImage { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string? RateId { get; set; }
        [ForeignKey(nameof(RateId))]
        public virtual Rate Rate { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public string? FreelancerId { get; set; }
        [ForeignKey("FreelancerId")]
        public virtual User? Freelancer { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<JobRequest>? JobRequests { get; set; }
    }

}
