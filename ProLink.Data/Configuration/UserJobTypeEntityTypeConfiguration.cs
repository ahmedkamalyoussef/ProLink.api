using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class UserJobTypeEntityTypeConfiguration : IEntityTypeConfiguration<UserJobType>
    {
        public void Configure(EntityTypeBuilder<UserJobType> builder)
        {
            builder
                .HasKey(uj => new { uj.JobId, uj.UserId });
            //builder
            //    .HasOne(uj => uj.User)
            //    .WithMany(uj => uj.Jobs)
            //    .HasForeignKey(uj => uj.JobId)
            //    .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasOne(uj => uj.Job)
                .WithMany(uj => uj.UserJobType)
                .HasForeignKey(uj => uj.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
