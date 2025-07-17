namespace FileBrowser.Data.Entities
{
    public class FileEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid FolderId { get; set; }
        public Folder? Folder { get; set; }
    }
}
