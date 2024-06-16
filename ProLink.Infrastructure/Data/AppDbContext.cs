using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProLink.Data.Configuration;
using ProLink.Data.Entities;

namespace ProLink.Data.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<React> Reacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserJobType> UserJobTypes { get; set; }
        public DbSet<UserPostType> UserPostTypes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            #region user
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            #endregion

            #region post
            new JobEntityTypeConfiguration().Configure(modelBuilder.Entity<Job>());
            #endregion

            #region jop request
            new JobRequestEntityTypeConfiguration().Configure(modelBuilder.Entity<JobRequest>());
            #endregion

            #region friend request
            new FriendRequestEntityTypeConfiguration().Configure(modelBuilder.Entity<FriendRequest>());
            #endregion

            #region comment
            new CommentEntityTypeConfiguration().Configure(modelBuilder.Entity<Comment>());
            #endregion

            #region likes
            new ReactEntityTypeConfiguration().Configure(modelBuilder.Entity<React>());
            #endregion

            #region notification
            new NotificationEntityTypeConfiguration().Configure(modelBuilder.Entity<Notification>());
            #endregion

            #region rate
            new RateEntityTypeConfiguration().Configure(modelBuilder.Entity<Rate>());
            #endregion
            
            #region message
            new MessageEntityTypeConfiguration().Configure(modelBuilder.Entity<Message>());
            #endregion

            #region UserFriend
            new UserFriendEntityTypeConfiguration().Configure(modelBuilder.Entity<UserFriend>());
            #endregion

            #region UserJobType
            new UserJobTypeEntityTypeConfiguration().Configure(modelBuilder.Entity<UserJobType>());
            #endregion

            #region UserPostType
            new UserPostTypeEntityTypeConfiguration().Configure(modelBuilder.Entity<UserPostType>());
            #endregion

            #region follower
            new FollowersEntityTypeConfiguration().Configure(modelBuilder.Entity<UserFollower>());
            #endregion

            #region Post
            new PostEntityTypeConfiguration().Configure(modelBuilder.Entity<Post>());
            #endregion
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
