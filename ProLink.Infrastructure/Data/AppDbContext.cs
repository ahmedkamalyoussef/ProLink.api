using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Entities;

namespace ProLink.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region user
            modelBuilder.Entity<User>()
               .HasMany(r => r.Friends)
               .WithOne()
               .HasForeignKey(r => r.FriendId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
               .HasMany(r => r.SentJobRequests)
               .WithOne(p => p.Sender)
               .HasForeignKey(r => r.SenderId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(r => r.ReceivedJobRequests)
               .WithOne(p => p.Receiver)
               .HasForeignKey(r => r.RecieverId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(r => r.SentFriendRequests)
               .WithOne(p => p.Sender)
               .HasForeignKey(r => r.SenderId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(r => r.ReceivedFriendRequests)
               .WithOne(p => p.Receiver)
               .HasForeignKey(r => r.ReceiverId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.Comments)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.Likes)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.ReceivedRates)
                .WithOne(u => u.Rated)
                .HasForeignKey(c => c.RatedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.Skills)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.Notifications)
                .WithOne(u => u.Receiver)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.SentMessages)
                .WithOne(u => u.Sender)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(c => c.ReceivedMessages)
                .WithOne(u => u.Receiver)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region post
            modelBuilder.Entity<Post>()
                .HasMany(p => p.JobRequests)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(c => c.Posts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(c => c.Comments)
                .WithOne(u => u.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasMany(c => c.Likes)
                .WithOne(u => u.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region jop request
            modelBuilder.Entity<JobRequest>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentJobRequests)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobRequest>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedJobRequests)
                .HasForeignKey(c => c.RecieverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobRequest>()
                .HasOne(c => c.Post)
                .WithMany(u => u.JobRequests)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region friend request
            modelBuilder.Entity<FriendRequest>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region likes
            modelBuilder.Entity<Like>()
                .HasOne(c => c.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(c => c.Post)
                .WithMany(u => u.Likes)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region notification
            modelBuilder.Entity<Notification>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.Notifications)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region rate
            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Rater)
                .WithMany(u => u.SentRates)
                .HasForeignKey(r => r.RaterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Rated)
                .WithMany(r=>r.ReceivedRates)
                .HasForeignKey(r => r.RatedId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
            
            #region skill
            modelBuilder.Entity<Skill>()
                .HasOne(c => c.User)
                .WithMany(u => u.Skills)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region comment
            modelBuilder.Entity<Message>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            base.OnModelCreating(modelBuilder);

            SeedRoles(modelBuilder);
           
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                  new IdentityRole() { Id = "4566b930-e1f9-4777-a945-7c4237260ed1", Name = "User", NormalizedName = "User" },
                  new IdentityRole() { Id = "39527e49-3edc-4ed1-8e55-b7715292bd08", Name = "Admin", NormalizedName = "Admin" }
                );

        }
    }
}
