using Smart_Librarian.Data;
using Smart_Librarian.Interfaces;
using System.Collections;

namespace Smart_Librarian.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainContext context;
        private Hashtable repositories;

        public UnitOfWork(MainContext context)
        {
            this.context = context;
            repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(context);
            repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }

}
