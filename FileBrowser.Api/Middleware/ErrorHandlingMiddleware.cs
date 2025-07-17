using FileBrowser.Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace FileBrowser.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                // Handle custom exceptions first
                case CustomException customEx:
                    response.Message = customEx.Message;
                    response.StatusCode = customEx.StatusCode;
                    context.Response.StatusCode = customEx.StatusCode;

                    break;
                default:
                    response.Message = "An internal server error occurred.";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        public class ErrorResponse
        {
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }
    }
}