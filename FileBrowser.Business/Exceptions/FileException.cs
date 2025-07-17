namespace FileBrowser.Business.Exceptions
{
    public class FileException : CustomException
    {
        public FileException(string message, int statusCode)
        : base(message, statusCode)
        {
        }
    }
}
