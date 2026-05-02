using FluentValidation;
using MediatR;

namespace AtTheMovies.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> Validators)
    {
        _validators = Validators;
    }

    public async Task<TResponse> Handle(
        TRequest request
        , RequestHandlerDelegate<TResponse> next
        , CancellationToken cancellationToken)
    {
       if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (validationResults.Count != 0)
            {
                throw new ValidationException(validationResults);
            }
        }

        return await next();
    }
}

