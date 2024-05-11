using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class FriendRequestEntityTypeConfiguration: IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
