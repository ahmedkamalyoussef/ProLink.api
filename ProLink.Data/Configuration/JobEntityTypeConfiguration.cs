using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;


namespace ProLink.Data.Configuration
{
    public class JobEntityTypeConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder
                .HasMany(j => j.JobRequests)
                .WithOne(jr => jr.Job)
                .HasForeignKey(jr => jr.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(p => p.Freelancer)
                .WithMany(c => c.CompletedJobs)
                .HasForeignKey(c => c.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.User)
                .WithMany(c => c.Jobs)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
