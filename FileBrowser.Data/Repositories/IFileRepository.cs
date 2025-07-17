using FileBrowser.Data.Entities;

namespace FileBrowser.Data.Repositories
{
    public interface IFileRepository : IRepository<FileEntity>
    {
        Task<IEnumerable<FileEntity>> SearchFilesAsync(string search, int top);
        Task<IEnumerable<FileEntity>> GetByFolderIdAsync(Guid folderId);
        Task<IEnumerable<FileEntity>> SearchFilesInFolderAsync(Guid folderId, string search, int top);
        Task<bool> FileExistsAsync(Guid folderId, string fileName);
    }
}
