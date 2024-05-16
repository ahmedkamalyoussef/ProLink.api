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
               .HasMany(r => r.Friends)
               .WithOne()
               .HasForeignKey(r => r.FriendId)
               .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasMany(r => r.Followers)
               .WithOne()
               .HasForeignKey(r => r.FollowerId)
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
                .HasMany(c => c.Likes)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.ReceivedRates)
                .WithOne(u => u.Rated)
                .HasForeignKey(c => c.RatedId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Skills)
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
        }
    }
}
