using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLink.Data.Entities;

namespace ProLink.Data.Configuration
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasOne(c => c.Receiver)
                .WithMany(u => u.Notifications)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentNotifications)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
