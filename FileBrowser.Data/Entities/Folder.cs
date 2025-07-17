namespace FileBrowser.Data.Entities
{
    public class Folder : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
        public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
    }
}
