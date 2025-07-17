using FileBrowser.Business.DTOs;

namespace FileBrowser.Business.Services
{
    public interface IFolderService
    {
        Task<FolderDto?> GetByFolderIdAsync(Guid id);
        Task<IEnumerable<FolderDto>> GetSubFoldersAsync(Guid parentId);
        Task<IEnumerable<FolderDto>> GetFoldersAsync();
        Task<FolderDto> AddFolderAsync(FolderDto folderDto);
        Task DeleteAsync(Guid id);
        Task<FolderDto> UpdateFolderAsync(FolderDto folderDto);
    }
}
