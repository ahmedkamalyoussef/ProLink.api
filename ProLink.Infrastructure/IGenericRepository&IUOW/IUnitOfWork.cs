using Castle.Core.Resource;
using ProLink.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Infrastructure.IGenericRepository_IUOW
{
    public interface IUnitOfWork
    {
        public IGenericRepository<User> User { get; set; }
        public IGenericRepository<Post> Post { get; set; }

        public IGenericRepository<FriendRequest> FriendRequest { get; set; }
        public IGenericRepository<JobRequest> JopRequest { get; set; }
        public IGenericRepository<Comment> Comment { get; set; }

        void CreateTransaction();
        void Commit();
        void Rollback();
        int Save();
    }
}
