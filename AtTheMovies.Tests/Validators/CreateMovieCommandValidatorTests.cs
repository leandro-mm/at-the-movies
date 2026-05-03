using AtTheMovies.Commands.Movies;
using AtTheMovies.Validators.Movies;
using FluentAssertions;

namespace AtTheMovies.Tests.Validators;

public class CreateMovieCommandValidatorTests
{
    private readonly CreateMovieCommandValidator _validator;

    public CreateMovieCommandValidatorTests()
    {
        _validator = new CreateMovieCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "Movie 1",
            Genre = "Action",
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Name));
    }

    [Fact]
    public void Validate_EmptyName_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "", // Invalid - empty
            Genre = "Action",
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Name));
    }

    [Fact]
    public void Validate_NameTooShort_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "A", // Invalid - too short
            Genre = "Action",
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(CreateMovieCommand.Name)
            && e.ErrorMessage.Contains("at least 3 characters"));

    }

    [Fact]
    public void Validate_NameTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = new string('a', 501), // Invalid - too long
            Genre = "Action",
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(CreateMovieCommand.Name)
            && e.ErrorMessage.Contains("Name cannot exceed"));

    }
    [Fact]
    public void Validate_EmptyGenre_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "Name", 
            Genre = "", // Invalid - empty
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => 
             e.PropertyName == nameof(CreateMovieCommand.Genre));
    }

    [Fact]
    public void Validate_GenreTooShort_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "Name", // Invalid - too short
            Genre = "A",
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(CreateMovieCommand.Genre)
            && e.ErrorMessage.Contains("at least 3 characters"));

    }

    [Fact]
    public void Validate_GenreTooLong_ShouldHaveError()
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = "name", 
            Genre = new string('a', 501), // Invalid - too long
            Description = "An action-packed movie."
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            e.PropertyName == nameof(CreateMovieCommand.Genre)
            && e.ErrorMessage.Contains("Genre cannot exceed"));

    }

    [Theory]
    [InlineData("Valid Name", "Valid Genre", "Valid Description")]
    [InlineData("Another Name", "Another Genre", "Another Description")]
    public void Validate_VariousCombinations_ShouldBeValid(
        string name,
        string genre,
        string description)
    {
        // Arrange
        var command = new CreateMovieCommand
        {
            Name = name,
            Genre = genre,
            Description = description
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);

    }
}