using AtTheMovies.Commands.Movies;
using AtTheMovies.Handlers.Movies;
using AtTheMovies.Tests.Fixtures;
using AtTheMovies.Validators.Movies;
using FluentAssertions;

namespace AtTheMovies.Tests.Commands;

public class CreateMovieCommandHandlerTests : IClassFixture<DbContextFixture>
{
    private readonly DbContextFixture _fixture;
    private const string FiveHundredFiveAs =
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
        "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"; // 505 A's

    public CreateMovieCommandHandlerTests(DbContextFixture fixture)
    {
        _fixture = fixture;
    }
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateMovie()
    {
        // Arrange
        var movieName = "New Movie";
        var movieDescription = "A new movie description";
        var movieGenre = "Action";

        var context = _fixture.CreateDbContext();
        var handler = new CreateMovieCommandHandler(context);
        var command = new CreateMovieCommand
        {
            Name = movieName,
            Description = movieDescription,
            Genre = movieGenre
        };

        // Act
        var movieId = await handler.Handle(command, CancellationToken.None);

        // Assert
        //Assert.True(movieId > 0); // traditional assertions
        movieId.Should().BeGreaterThan(0);

        var movie = await context.Movies.FindAsync(movieId);
        movie.Should().NotBeNull();
        movie.Name.Should().Be(movieName);
        movie.Description.Should().Be(movieDescription);
        movie.Genre.Should().Be(movieGenre);
        movie.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void CreateMovieCommandValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange        
        var validator = new CreateMovieCommandValidator();
        var command = new CreateMovieCommand
        {
            Name = "", // Invalid - empty
            Description = "A new movie description",
            Genre = "Action"
        };

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
         e.PropertyName == nameof(CreateMovieCommand.Name) && e.ErrorMessage.Contains("required"));
    }

    [Theory]
    [InlineData("", "Action", "Description", nameof(CreateMovieCommand.Name))] // Empty name
    [InlineData("A", "Action", "Description", nameof(CreateMovieCommand.Name))] // Name too short
    [InlineData(FiveHundredFiveAs, "Action", "Description", nameof(CreateMovieCommand.Name))] // Name too long
    [InlineData("Valid Name", "", "Description", nameof(CreateMovieCommand.Genre))] // Empty genre
    [InlineData("Valid Name", "A", "Description", nameof(CreateMovieCommand.Genre))] // Genre too short
    [InlineData("Valid Name", FiveHundredFiveAs, "Description", nameof(CreateMovieCommand.Genre))] // Genre too long
    [InlineData("Valid Name", "Action", FiveHundredFiveAs, nameof(CreateMovieCommand.Description))] // Description too long
    public void CreateMovieCommandValidator_InvalidCommand_ShouldHaveValidationErrorForProperty(
        string name,
        string genre,
        string description,
        string expectedErrorProperty)
    {
        // Arrange
        var validator = new CreateMovieCommandValidator();
        var command = new CreateMovieCommand
        {
            Name = name,
            Genre = genre,
            Description = description
        };

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == expectedErrorProperty);
    }

    [Fact]
    public void CreateMovieCommandValidator_ValidCommand_ShouldHaveNoValidationErrors()
    {
        // Arrange
        var validator = new CreateMovieCommandValidator();
        var command = new CreateMovieCommand
        {
            Name = "Inception",
            Genre = "Sci-Fi",
            Description = "A thief who steals corporate secrets through dream-sharing technology."
        };

        // Act
        var result = validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}