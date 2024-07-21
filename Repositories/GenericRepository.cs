using Microsoft.EntityFrameworkCore;
using Smart_Librarian.Data;
using Smart_Librarian.Interfaces;
using System.Linq.Expressions;

namespace Smart_Librarian.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly MainContext context;

        public GenericRepository(MainContext context)
        {
            this.context = context;
        }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
        }
        public async Task AddAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
            context.SaveChanges();
        }

        public TEntity? GetId(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity?> GetIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return context.Set<TEntity>().FirstOrDefault(filter);
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return context.Set<TEntity>().Where(filter).ToList();
        }

        public IEnumerable<TEntity> GetListWithIncludes(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }


        public async Task<IEnumerable<TEntity>> GetListWithIncludesAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = context.Set<TEntity>().Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public int Count()
        {
            return context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<TEntity>().CountAsync();
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public Task UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().UpdateRange(entities);
            context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }
        public Task RemoveAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
            context.SaveChanges();
        }

        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return context.Set<TEntity>().Any(filter);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await context.Set<TEntity>().AnyAsync(filter);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }

}
