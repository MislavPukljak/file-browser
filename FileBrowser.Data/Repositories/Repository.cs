using FileBrowser.Data.Context;
using FileBrowser.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileBrowser.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly FileBrowserDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(FileBrowserDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
