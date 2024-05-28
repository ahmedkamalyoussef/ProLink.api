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
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentJobRequests)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedJobRequests)
                .HasForeignKey(c => c.RecieverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
