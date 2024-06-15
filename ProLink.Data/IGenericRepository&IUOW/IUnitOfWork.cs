using ProLink.Data.Entities;

namespace ProLink.Infrastructure.IGenericRepository_IUOW
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> User { get; set; }
        IGenericRepository<Job> Job { get; set; }
        IGenericRepository<React> React { get; set; }
        IGenericRepository<FriendRequest> FriendRequest { get; set; }
        IGenericRepository<JobRequest> JobRequest { get; set; }
        IGenericRepository<Comment> Comment { get; set; }
        IGenericRepository<Rate> Rate { get; set; }
        IGenericRepository<Message> Message { get; set; }
        IGenericRepository<Notification> Notification { get; set; }
        IGenericRepository<UserFriend> UserFriend { get; set; }
        IGenericRepository<UserFollower> UserFollower { get; set; }
        IGenericRepository<Post> Post { get; set; }
        IGenericRepository<UserJobType> UserJobType { get; set; }

        Task CreateTransactionAsync();
        Task CommitAsync();
        Task CreateSavePointAsync(string point);
        Task RollbackAsync();
        Task RollbackToSavePointAsync(string point);
        Task<int> SaveAsync();
    }
}
