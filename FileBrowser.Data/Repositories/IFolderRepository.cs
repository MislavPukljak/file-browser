using FileBrowser.Data.Entities;

namespace FileBrowser.Data.Repositories
{
    public interface IFolderRepository : IRepository<Folder>
    {
        Task<IEnumerable<Folder>> GetSubFoldersAsync(Guid? folderId);
        Task<bool> FolderExistsAsync(Guid? parentFolderId, string folderName);
        Task<bool> ParentFolderExistsAsync(Guid? parentFolderId);
    }
}
