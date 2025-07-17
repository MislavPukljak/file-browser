namespace FileBrowser.Business.DTOs
{
    public class FileDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid FolderId { get; set; }
    }
}
