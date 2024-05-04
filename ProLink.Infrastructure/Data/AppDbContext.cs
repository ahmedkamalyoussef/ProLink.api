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

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Rater)
                .WithMany(u => u.Rates)
                .HasForeignKey(r => r.RaterId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Rated)
                .WithMany()
                .HasForeignKey(r => r.RatedId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
           
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData
                (
                  new IdentityRole() { Name = "User", NormalizedName = "User" },
                  new IdentityRole() { Name = "Admin", NormalizedName = "Admin" }
                );

        }
    }
}
