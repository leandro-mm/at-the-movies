using AtTheMovies.Commands.Movies;
using AtTheMovies.Handlers.Movies;
using AtTheMovies.Tests.Fixtures;
using FluentAssertions;

namespace AtTheMovies.Tests.Commands;

public class UpdateMovieCommandHandlerTests : IClassFixture<DbContextFixtureWithData>
{
    private readonly DbContextFixtureWithData _fixture;
    private readonly UpdateMovieCommandHandler _handler;
    
    public UpdateMovieCommandHandlerTests(DbContextFixtureWithData fixture)
    {
        _fixture = fixture;
        _handler = new UpdateMovieCommandHandler(_fixture.Context);
    }

    [Fact]
    public async Task Handle_ExistingMovie_ShouldUpdateSuccessfully()
    {
        // Arrange
        var existingMovie = _fixture.Context.Movies.First();
        var updatedName = "Updated Movie Name";
        var updatedDescription = "Updated Movie Description";
        var updatedGenre = "Updated Genre";

        var command = new UpdateMovieCommand
        {
            Id = existingMovie.Id,
            Name = updatedName,
            Description = updatedDescription,
            Genre = updatedGenre
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedMovie = await _fixture.Context.Movies.FindAsync(existingMovie.Id);
        updatedMovie.Should().NotBeNull();
        updatedMovie.Name.Should().Be(updatedName);
        updatedMovie.Description.Should().Be(updatedDescription);
        updatedMovie.Genre.Should().Be(updatedGenre);
    }

    [Fact]
    public async Task Handle_NonExistingMovie_ShouldReturnFalse()
    {
        // Arrange
        var command = new UpdateMovieCommand
        {
            Id = 0, // Assuming 0 is an ID that does not exist in the database
            Name = "Non Existing Movie",
            Description = "Non Existing Movie Description",
            Genre = "Non Existing Genre"
        };

        // Act 
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_UpdateWithSameData_ShouldNotChangeMovie()
    {
        // Arrange
        var existingMovie = _fixture.Context.Movies.First();

        var command = new UpdateMovieCommand
        {
            Id = existingMovie.Id,
            Name = existingMovie.Name,
            Description = existingMovie.Description,
            Genre = existingMovie.Genre
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var updatedMovie = await _fixture.Context.Movies.FindAsync(existingMovie.Id);

        // Assert
        updatedMovie.Should().NotBeNull();        
        updatedMovie.Name.Should().Be(existingMovie.Name);
        updatedMovie.Description.Should().Be(existingMovie.Description);
        updatedMovie.Genre.Should().Be(existingMovie.Genre);
    }

    [Fact]
    public async Task Handle_UpdateWithSameData_ShouldReturnTrue()
    {
        // Arrange
        var existingMovie = _fixture.Context.Movies.First();

        var command = new UpdateMovieCommand
        {
            Id = existingMovie.Id,
            Name = existingMovie.Name,
            Description = existingMovie.Description,
            Genre = existingMovie.Genre
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);        

        // Assert        
        result.Should().BeTrue();
    }
}