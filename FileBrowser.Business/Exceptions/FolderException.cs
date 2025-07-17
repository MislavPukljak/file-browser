namespace FileBrowser.Business.Exceptions
{
    public class FolderException : CustomException
    {
        public FolderException(string message, int statusCode)
        : base(message, statusCode)
        {
        }
    }
}
