using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class UserPostTypeEntityTypeConfiguration : IEntityTypeConfiguration<UserPostType>
    {
        public void Configure(EntityTypeBuilder<UserPostType> builder)
        {
            //builder
            //    .HasKey(up => new {up.PostId , up.UserId });
            builder
                .HasOne(up => up.Post)
                .WithMany(up => up.UserPostTypes)
                .HasForeignKey(up => up.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
