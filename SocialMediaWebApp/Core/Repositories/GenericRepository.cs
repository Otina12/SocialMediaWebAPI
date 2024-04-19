using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.Data;

namespace SocialMediaWebApp.Core.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbSet = context.Set<T>();
        }


        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<bool> Delete(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            if(entity is null)
            {
                return false;
            }

            dbSet.Remove(entity);
            return true;
        }

    }
}
