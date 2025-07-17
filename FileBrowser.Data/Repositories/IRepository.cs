using FileBrowser.Data.Entities;

namespace FileBrowser.Data.Repositories
{
    public interface IRepository<TEntity> 
        where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
    }
}
