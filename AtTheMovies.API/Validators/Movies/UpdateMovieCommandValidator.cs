using AtTheMovies.Commands.Movies;
using FluentValidation;

namespace AtTheMovies.Validators.Movies;

public class UpdateMovieCommandValidator
    : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(MovieConstraints.MinLength).WithMessage($"Name must be at least {MovieConstraints.MinLength} characters long.")
            .MaximumLength(MovieConstraints.MaxLength).WithMessage($"Name cannot exceed {MovieConstraints.MaxLength} characters.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required.")
            .MinimumLength(MovieConstraints.MinLength).WithMessage($"Genre must be at least {MovieConstraints.MinLength} characters long.")
            .MaximumLength(MovieConstraints.MaxLength).WithMessage($"Genre cannot exceed {MovieConstraints.MaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(MovieConstraints.MaxDescriptionLength).WithMessage($"Description cannot exceed {MovieConstraints.MaxDescriptionLength} characters.");

    }
}