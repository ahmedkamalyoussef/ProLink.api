using ProLink.Data.Entities;

namespace ProLink.Infrastructure.IGenericRepository_IUOW
{
    public interface IUnitOfWork
    {
        public IGenericRepository<User> User { get; set; }
        public IGenericRepository<Post> Post { get; set; }
        public IGenericRepository<Like> Like { get; set; }
        public IGenericRepository<Skill> Skill { get; set; }
        public IGenericRepository<FriendRequest> FriendRequest { get; set; }
        public IGenericRepository<JobRequest> JopRequest { get; set; }
        public IGenericRepository<Comment> Comment { get; set; }
        public IGenericRepository<Rate> Rate { get; set; }
        public IGenericRepository<Message> Message { get; set; }
        public IGenericRepository<Notification> Notification { get; set; }

        void CreateTransaction();
        void Commit();
        void CreateSavePoint(string point);
        void Rollback();
        void RollbackToSavePoint(string point);
        int Save();
    }
}
