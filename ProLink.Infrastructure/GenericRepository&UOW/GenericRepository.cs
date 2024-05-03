using Microsoft.EntityFrameworkCore;
using ProLink.Data.Specification;
using ProLink.Infrastructure.Data;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using System.Linq.Expressions;

namespace ProLink.Infrastructure.GenericRepository_UOW
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext context;
        public GenericRepository(AppDbContext context)  
        {
            this.context = context;
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)

        {
            return context.Set<T>().Where(expression);
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> expression)
        {
            return await context.Set<T>().FirstOrDefaultAsync(expression);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return context.Set<T>().ToList();
        }
        public T GetById(string id)
        {

            return context.Set<T>().Find(id);
        }
        public void Update(T entity)
        {
            context.Attach(entity);
            context.Entry<T>(entity).State = EntityState.Modified;
        }
        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();

        }

        public IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
        }
    }
}
