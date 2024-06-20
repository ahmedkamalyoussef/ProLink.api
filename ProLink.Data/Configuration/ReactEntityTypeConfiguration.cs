using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class ReactEntityTypeConfiguration : IEntityTypeConfiguration<React>
    {
        public void Configure(EntityTypeBuilder<React> builder)
        {
            builder
                .HasOne(c => c.User)
                .WithMany(u => u.Reacts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
