using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class FollowersEntityTypeConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder
                .HasKey(c => new { c.FollowerId, c.UserId });
        }
    }
}
