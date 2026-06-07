using System.Net;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace SisAlq.Api.Shared.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var traceId = context.TraceIdentifier;

        var (status, error, message) = ex switch
        {
            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx
                && pgEx.SqlState == "23505"
                => (409, "Conflict", "Ya existe un registro con esos datos."),

            DbUpdateException dbEx when dbEx.InnerException is PostgresException pgEx
                && pgEx.SqlState == "23503"
                => (400, "Bad Request", "Referencia inválida. Verifique los datos enviados."),

            DbUpdateException
                => (500, "Database Error", "Error al guardar los datos."),

            NpgsqlException
                => (503, "Service Unavailable", "No se pudo conectar a la base de datos."),

            TimeoutException
                => (504, "Gateway Timeout", "La operación tardó demasiado. Intente nuevamente."),

            KeyNotFoundException
                => (404, "Not Found", ex.Message),

            InvalidOperationException
                => (400, "Bad Request", ex.Message),

            _ => (500, "Internal Server Error", "Ocurrió un error inesperado. Contacte al administrador.")
        };

        context.Response.StatusCode = status;

        object response = _env.IsDevelopment()
            ? new { status, error, message, traceId, detail = ex.ToString() }
            : new { status, error, message, traceId };

        await context.Response.WriteAsJsonAsync(response);
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static WebApplication UseErrorHandling(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        return app;
    }
}