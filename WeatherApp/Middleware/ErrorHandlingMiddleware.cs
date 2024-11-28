using System.Net;
using WeatherApp.Logging;
using WeatherApp.Enums;

namespace WeatherApp.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly WeatherApp.Logging.ILogger _logger;

        public ErrorHandlingMiddleware(WeatherApp.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.Error("EX");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, errorType, errorMessage) = ex switch
            {
                ArgumentException or InvalidOperationException =>
                    (HttpStatusCode.BadRequest, ErrorHadlerType.BadRequest, "Bad request."),
                KeyNotFoundException =>
                    (HttpStatusCode.NotFound, ErrorHadlerType.NotFound, "Not found."),
                _ =>
                    (HttpStatusCode.InternalServerError, ErrorHadlerType.InternalServerError, "InternalServerError")
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                context.Response.StatusCode,
                ErrorType = errorType.ToString(),
                Message = errorMessage
            };

            _logger.Error($"Error: {errorType} - {errorMessage}");

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorResponse));
        }
    }
}
