using FluentValidation;
using FluentValidation.Results;
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
            var validationResults = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);
                if (result != null && result.Errors != null)
                {
                    validationResults.AddRange(result.Errors);
                }
            }           

            if (validationResults.Count != 0)
            {
                throw new ValidationException(validationResults);
            }
        }

        return await next();
    }
}

