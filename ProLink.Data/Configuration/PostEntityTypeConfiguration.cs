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
                .HasMany(p => p.UserPostTypes)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
