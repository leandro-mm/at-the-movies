using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AtTheMovies.Middleware;

public class GlobalExceptionHandler :IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
            _logger.LogError(exception, "An unhandled exception occurred while processing the request.");
            
            var problemDetails = new ProblemDetails
            {
                Status = exception switch
                {
                    ValidationException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = exception switch
                {
                    ValidationException => "Validation Error",
                    KeyNotFoundException => "Resource Not Found",
                    _ => "An unexpected error occurred"
                },
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            if(exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = 
                    validationException.ValidationResult.MemberNames
                    .ToDictionary(name => name, name => new[] { 
                        validationException.ValidationResult.ErrorMessage 
                        });
            }
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
    }
}
