using Microsoft.EntityFrameworkCore;
using ProLink.Data.Consts;
using ProLink.Data.Data;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using System.Linq.Expressions;

namespace ProLink.Infrastructure.GenericRepository_UOW
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)  
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, string? direction = null)
        {
            IQueryable<T> query=_context.Set<T>().Where(expression);
            if (orderBy != null)
            {
                if(direction==OrderDirection.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return query.ToList();
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);

        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, string? direction = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (orderBy != null)
            {
                if (direction == OrderDirection.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return query.ToList();
        }
        public T GetById(string id)
        {

            return _context.Set<T>().Find(id);
        }
        public void Update(T entity)
        {
            _context.Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Modified;
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }
    }
}
