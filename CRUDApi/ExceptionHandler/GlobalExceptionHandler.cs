using System.Net;
using System.Text.Json;

namespace CRUDApi.ExceptionHandler
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = "Internal Server Error.";

            switch (exception)
            {
                case InvalidOperationException:
                    status = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case ArgumentException:
                    status = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                default:
                    message = exception.Message;
                    break;
            }
            var result = JsonSerializer.Serialize(new
            {
                success = false,
                error = message,
                statusCode = (int)status
            });

            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
