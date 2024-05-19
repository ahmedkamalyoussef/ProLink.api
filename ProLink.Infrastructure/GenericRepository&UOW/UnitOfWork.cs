using Microsoft.EntityFrameworkCore.Storage;
using ProLink.Infrastructure.Data;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using ProLink.Data.Entities;

namespace ProLink.Infrastructure.GenericRepository_UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction transaction;

        private AppDbContext _context;
        public virtual IGenericRepository<User> User { get; set; }
        public virtual IGenericRepository<Post> Post { get; set; }
        public virtual IGenericRepository<FriendRequest> FriendRequest { get; set; }
        public virtual IGenericRepository<JobRequest> JopRequest { get; set; }
        public virtual IGenericRepository<Comment> Comment { get; set; }
        public virtual IGenericRepository<Like> Like { get; set; }
        //public virtual IGenericRepository<Skill> Skill { get; set; }
        public virtual IGenericRepository<Rate> Rate { get; set; }
        public virtual IGenericRepository<Message> Message { get; set; }
        public virtual IGenericRepository<Notification> Notification { get; set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            User = new GenericRepository<User>(_context);
            FriendRequest = new GenericRepository<FriendRequest>(_context);
            JopRequest = new GenericRepository<JobRequest>(_context);
            Comment = new GenericRepository<Comment>(_context);
            Post= new GenericRepository<Post>(_context);
            Like = new GenericRepository<Like>(_context);
            Rate = new GenericRepository<Rate>(_context);
            Message = new GenericRepository<Message>(_context);
            Notification = new GenericRepository<Notification>(_context);
        }

        public void CreateTransaction()
        {
            transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void CreateSavePoint(string point)
        {
            transaction.CreateSavepoint(point);
        }
        public void Rollback()
        {
            transaction.Rollback();

        }

        public void RollbackToSavePoint(string point)
        {
            transaction.RollbackToSavepoint(point);

        }


        public int Save()
        {
            return _context.SaveChanges();
        }

    }
}
