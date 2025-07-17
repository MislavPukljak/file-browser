
using FileBrowser.Data.Context;

namespace FileBrowser.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileBrowserDbContext _context;

        public UnitOfWork(FileBrowserDbContext context)
        {
            _context = context;
        }

        public IFileRepository Files => new FileRepository(_context);

        public IFolderRepository Folder => new FolderRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
