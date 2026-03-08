using System.Net;
using System.Text.Json;
using TodoApp.Application.Exceptions;

namespace TodoApp.api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Continue le chemin normal
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); // Log l'erreur dans la console
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ErrorResponse { StatusCode = context.Response.StatusCode, Message = ex.Message, Details = ex.StackTrace?.ToString() }
                : new ErrorResponse { StatusCode = context.Response.StatusCode, Message = "Une erreur interne est survenue." };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}