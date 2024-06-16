using ProLink.Data.Consts;

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

        public virtual Rate Rate { get; set; }

        public string? FreelancerId { get; set; }

        public virtual User? Freelancer { get; set; }
        public virtual ICollection<UserJobType> UserJobType { get; set; }

        public virtual ICollection<JobRequest>? JobRequests { get; set; }
    }

}
