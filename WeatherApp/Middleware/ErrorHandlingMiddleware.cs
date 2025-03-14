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
                _logger.Error("EXception found by middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.Error($"Error handling in progress by middleware: {ex.Message}");

            context.Response.Clear();
            context.Response.Redirect("/Home/Error", false);

            return Task.CompletedTask;
        }
    }
}
