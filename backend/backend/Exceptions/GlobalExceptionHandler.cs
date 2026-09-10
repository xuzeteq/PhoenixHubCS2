using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using backend.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace backend.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            System.Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, errorCode, message) = exception switch
            {
                Domain.Exceptions.BaseException ex => (ex.StatusCode, ex.ErrorCode, ex.Message),
                DbUpdateException => (500, "DATABASE_ERROR", "Ошибка базы данных"),
                ArgumentException ex => (400, "INVALID_ARGUMENT", ex.Message),
                _ => (500, "INTERNAL_ERROR", "Внутренняя ошибка сервера")
            };

            if (statusCode >= 500)
            {
                _logger.LogError(exception,
                    "Ошибка сервера [{StatusCode}]: {ErrorCode} - {Message}",
                    statusCode, errorCode, message);
            }
            else
            {
                _logger.LogWarning(
                    "Ошибка [{StatusCode}]: {ErrorCode} - {Message} | Путь: {Path}",
                    statusCode, errorCode, message, httpContext.Request.Path);
            }

            httpContext.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = GetTitle(statusCode),
                Detail = message,
                Instance = httpContext.Request.Path
            };

            problem.Extensions["errorCode"] = errorCode;
            problem.Extensions["traceId"] = httpContext.TraceIdentifier;

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }

        private static string GetTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }
}