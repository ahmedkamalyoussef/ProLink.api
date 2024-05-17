using System.Linq.Expressions;

namespace ProLink.Infrastructure.IGenericRepository_IUOW
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(string id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? orderBy = null, string? direction = null);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void Update(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, string? direction = null);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> expression);
    }
}
