using FileBrowser.Api.Middleware;

namespace FileBrowser.Api.Extensions
{
    public static class ErrorHandlingExtension
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            return app;
        }
    }
}
