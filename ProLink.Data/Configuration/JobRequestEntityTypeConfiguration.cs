using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class JobRequestEntityTypeConfiguration : IEntityTypeConfiguration<JobRequest>
    {
        public void Configure(EntityTypeBuilder<JobRequest> builder)
        {
            builder
                .HasOne(jr => jr.Sender)
                .WithMany(u => u.SentJobRequests)
                .HasForeignKey(jr => jr.SenderId)
                .OnDelete(DeleteBehavior.NoAction); // No action on Sender

            builder
                .HasOne(jr => jr.Job)
                .WithMany(j => j.JobRequests)
                .HasForeignKey(jr => jr.JobId)
                .OnDelete(DeleteBehavior.Cascade); // No action on Job
        }
    }
}
