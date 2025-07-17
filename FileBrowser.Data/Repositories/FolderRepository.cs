using FileBrowser.Data.Context;
using FileBrowser.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileBrowser.Data.Repositories
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(FileBrowserDbContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<Folder>> GetSubFoldersAsync(Guid? folderId)
        {
            return await _dbSet
                .Where(x => x.ParentFolderId == folderId)
                .ToListAsync();
        }

        public override async Task<Folder?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.SubFolders)
                .Include(f => f.Files)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> FolderExistsAsync(Guid? parentFolderId, string folderName)
        {
            return await _dbSet
                .AnyAsync(x => (x.ParentFolderId == parentFolderId || x.ParentFolderId == null) && x.Name == folderName);
        }

        public async Task<bool> ParentFolderExistsAsync(Guid? parentFolderId)
        {
            return await _dbSet
                .AnyAsync(x => x.Id == parentFolderId);
        }
    }
}
