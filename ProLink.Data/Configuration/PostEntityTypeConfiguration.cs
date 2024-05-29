using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class PostEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasMany(c => c.Comments)
                .WithOne(u => u.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Reacts)
                .WithOne(u => u.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
