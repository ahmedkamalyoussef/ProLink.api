using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;
using System.Reflection.Emit;

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
                .HasOne(j => j.Rate)
                .WithOne(r=>r.RatedJob)
                .HasForeignKey<Job>(j=>j.RateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(j => j.Freelancer)
                .WithMany()
                .HasForeignKey(j => j.FreelancerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(j => j.User)
                .WithMany(u=>u.Jobs)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
