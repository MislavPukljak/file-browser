namespace FileBrowser.Data.Repositories
{
    public interface IUnitOfWork
    {
        IFileRepository Files {  get; }
        IFolderRepository Folder { get; }
        Task<int> SaveChangesAsync();
    }
}
