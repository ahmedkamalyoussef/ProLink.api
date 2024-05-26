using Microsoft.EntityFrameworkCore.Storage;
using ProLink.Infrastructure.Data;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using ProLink.Data.Entities;
using System.Threading.Tasks;

namespace ProLink.Infrastructure.GenericRepository_UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction transaction;

        private readonly AppDbContext _context;
        public virtual IGenericRepository<User> User { get; set; }
        public virtual IGenericRepository<Post> Post { get; set; }
        public virtual IGenericRepository<FriendRequest> FriendRequest { get; set; }
        public virtual IGenericRepository<JobRequest> JobRequest { get; set; }
        public virtual IGenericRepository<Comment> Comment { get; set; }
        public virtual IGenericRepository<Like> Like { get; set; }
        public virtual IGenericRepository<Rate> Rate { get; set; }
        public virtual IGenericRepository<Message> Message { get; set; }
        public virtual IGenericRepository<Notification> Notification { get; set; }
        public virtual IGenericRepository<UserFriend> UserFriend { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            User = new GenericRepository<User>(_context);
            FriendRequest = new GenericRepository<FriendRequest>(_context);
            JobRequest = new GenericRepository<JobRequest>(_context);
            Comment = new GenericRepository<Comment>(_context);
            Post = new GenericRepository<Post>(_context);
            Like = new GenericRepository<Like>(_context);
            Rate = new GenericRepository<Rate>(_context);
            Message = new GenericRepository<Message>(_context);
            Notification = new GenericRepository<Notification>(_context);
            UserFriend = new GenericRepository<UserFriend>(_context);
        }

        public async Task CreateTransactionAsync()
        {
            transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await transaction.CommitAsync();
        }

        public async Task CreateSavePointAsync(string point)
        {
            await transaction.CreateSavepointAsync(point);
        }

        public async Task RollbackAsync()
        {
            await transaction.RollbackAsync();
        }

        public async Task RollbackToSavePointAsync(string point)
        {
            await transaction.RollbackToSavepointAsync(point);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
