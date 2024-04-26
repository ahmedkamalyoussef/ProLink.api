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
        

        void CreateTransaction();
        void Commit();
        void Rollback();
        int Save();
    }
}
