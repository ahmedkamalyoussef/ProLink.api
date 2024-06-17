using ProLink.Data.Consts;
using System.ComponentModel.DataAnnotations;

namespace ProLink.Data.Entities
{
    public class Job
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string? PostImage { get; set; }

        public Status Status { get; set; }  
        public DateTime DateCreated { get; set; }
        public string? RateId { get; set; }
        public virtual Rate? Rate { get; set; }

        public string? FreelancerId { get; set; }
        public virtual User? Freelancer { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<JobRequest>? JobRequests { get; set; }
    }

}
