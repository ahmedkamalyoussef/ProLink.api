using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(u => u.Friends)
                .WithOne(uf => uf.User)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(u => u.Followers)
                .WithOne(uf => uf.User)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder
               .HasMany(r => r.SentJobRequests)
               .WithOne(p => p.Sender)
               .HasForeignKey(r => r.SenderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(r => r.ReceivedJobRequests)
               .WithOne(p => p.Receiver)
               .HasForeignKey(r => r.RecieverId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(r => r.SentFriendRequests)
               .WithOne(p => p.Sender)
               .HasForeignKey(r => r.SenderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(r => r.ReceivedFriendRequests)
               .WithOne(p => p.Receiver)
               .HasForeignKey(r => r.ReceiverId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Comments)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Reacts)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder
                .HasMany(c => c.Notifications)
                .WithOne(u => u.Receiver)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.SentMessages)
                .WithOne(u => u.Sender)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.ReceivedMessages)
                .WithOne(u => u.Receiver)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Posts)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder
            //    .HasMany(c => c.Jobs)
            //    .WithOne(u => u.User)
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
