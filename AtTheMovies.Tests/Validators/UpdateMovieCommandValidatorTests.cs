using AtTheMovies.Commands.Movies;
using AtTheMovies.Validators.Movies;
using FluentAssertions;

namespace AtTheMovies.Tests.Validators;

public class UpdateMovieCommandValidatorTests
{
    private readonly UpdateMovieCommandValidator _validator;

    public UpdateMovieCommandValidatorTests()
    {
        _validator = new UpdateMovieCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        //Arrange
        var command = new UpdateMovieCommand
        {
            Id = 1,
            Name = "Valid Title",
            Genre = "Action",            
            Description = "Valid description"
        };

        //Act
        var result = _validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_InvalidCommand_ShouldHaveErrors()
    {
        //Arrange
        var command = new UpdateMovieCommand
        {
            Id = 0, // Invalid Id
            Name = "Valid Title",
            Genre = "Action",            
            Description = "Valid description"
        };

        //Act
        var result = _validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validate_NegativeId_ShouldHaveErrors()
    {
        //Arrange
        var command = new UpdateMovieCommand
        {
            Id = -1, // Invalid Id
            Name = "Valid Title",
            Genre = "Action",            
            Description = "Valid description"
        };

        //Act
        var result = _validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}