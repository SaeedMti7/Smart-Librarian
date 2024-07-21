using System.Linq.Expressions;

namespace Smart_Librarian.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity);
      
        void AddRange(IEnumerable<T> entities);
        T? GetId(int id);
        Task<T?> GetIdAsync(int id);
        T? Get(Expression<Func<T, bool>> filter);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetList(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetListWithIncludes(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetListWithIncludesAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        int Count();
        Task<int> CountAsync();
        void Update(T entity);
        Task UpdateAsync(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity); 
        Task RemoveAsync(T entity);

        void RemoveRange(IEnumerable<T> entities);
        bool Exists(Expression<Func<T, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
        void Dispose();
    }
}
