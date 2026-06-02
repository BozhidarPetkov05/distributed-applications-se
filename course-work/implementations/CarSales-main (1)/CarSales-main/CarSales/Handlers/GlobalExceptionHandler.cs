using CarSales.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Handlers
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
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred in the Car Sales API: {Message}", exception.Message);

            var (statusCode, title, detail) = exception switch
            {
                BadRequestException => (StatusCodes.Status400BadRequest, "Invalid request. Your request contains invalid data or your request is invalid.", exception.Message),
                NotFoundException => (StatusCodes.Status404NotFound, "The resource was not found.", exception.Message),
                ForbidException => (StatusCodes.Status403Forbidden, "You don't have access to this resource.", exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "You are not authorized. You need to be logged for this operation.", exception.Message),

                _ => (StatusCodes.Status500InternalServerError, "Internal server error occurred.", "A generic error occurred on the server.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}