using FileBrowser.Business.DTOs;

namespace FileBrowser.Business.Services
{
    public interface IFileService
    {
        Task<FileDto> GetByFileIdAsync(Guid id);
        Task<IEnumerable<FileDto>> GetAllFilesAsync();
        Task<IEnumerable<FileDto>> GetByFolderIdAsync(Guid folderId);
        Task<IEnumerable<FileDto>> SearchFilesAsync(string search, int top = 10);
        Task<IEnumerable<FileDto>> SearchFilesInFolderAsync(Guid folderId, string search, int top = 10);
        Task<FileDto> AddFileAsync(FileDto fileDto);
        Task DeleteFileAsync(Guid id);
        Task<FileDto> UpdateFileAsync(FileDto fileDto);
    }
}
