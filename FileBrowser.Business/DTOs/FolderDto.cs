namespace FileBrowser.Business.DTOs
{
    public class FolderDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentFolderId { get; set; }
        public List<FileDto?> Files { get; set; } = new();
        public List<FolderDto?> SubFolders { get; set; } = new();
    }
}
