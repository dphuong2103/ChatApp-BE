using System.Net;
using System.Text.Json;

namespace ChatAppBackEnd.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception err)
            {
                _logger.LogError(err, err.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problem = new()
                {
                    Detail = "An internal server has occured",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Type = "Server error"
                };
                string json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
                context.Response.ContentType = "application/json";
            }

        }
    }
    public class ProblemDetails
    {
        public int Status { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
