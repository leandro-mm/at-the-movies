using System.Diagnostics;
using MediatR;

namespace AtTheMovies.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> Logger)
    {
        _logger = Logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName} with content {@Request}",
            typeof(TRequest).Name, request);

        var stopwatch = Stopwatch.StartNew();
        var response = await next();

        stopwatch.Stop();

        _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms with response {@Response}",
            typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, response);

        return response;
    }
}