using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProLink.Infrastructure.Data;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using ProLink.Data.Entities;

namespace ProLink.Infrastructure.GenericRepository_UOW
{
    public class UnitOfWork
    {
        private IDbContextTransaction transaction;

        private AppDbContext _context;
        public virtual IGenericRepository<User> User { get; set; }
        public virtual IGenericRepository<FriendRequest> FriendRequest { get; set; }
        public virtual IGenericRepository<JobRequest> JopRequest { get; set; }
        public virtual IGenericRepository<Comment> Comment { get; set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            
        }

        public void CreateTransaction()
        {
            transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();

        }


        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
