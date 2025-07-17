using FileBrowser.Data.Context;
using FileBrowser.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileBrowser.Data.Repositories
{
    public class FileRepository : Repository<FileEntity>, IFileRepository
    {
        public FileRepository(FileBrowserDbContext context) 
            : base(context)
        {
        }

        public async Task<bool> FileExistsAsync(Guid folderId, string fileName)
        {
            return await _dbSet
                .AnyAsync(x => x.FolderId == folderId && x.Name == fileName);
        }

        public async Task<IEnumerable<FileEntity>> GetByFolderIdAsync(Guid folderId)
        {
            return await _dbSet
                .Where(x => x.FolderId == folderId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<FileEntity>> SearchFilesAsync(string search, int top)
        {
            return await _dbSet
                .Where(x => x.Name.StartsWith(search))
                .OrderBy(x => x.Name)
                .Take(top)
                .ToListAsync();
        }

        public async Task<IEnumerable<FileEntity>> SearchFilesInFolderAsync(Guid folderId, string search, int top)
        {
            return await _dbSet
                .Where(x => x.FolderId == folderId && x.Name.StartsWith(search))
                .OrderBy(x => x.Name)
                .Take(top)
                .ToListAsync();
        }
    }
}
