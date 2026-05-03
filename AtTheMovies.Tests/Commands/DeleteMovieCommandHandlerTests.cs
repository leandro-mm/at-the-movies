using AtTheMovies.Commands.Movies;
using AtTheMovies.Handlers.Movies;
using AtTheMovies.Tests.Fixtures;
using FluentAssertions;

namespace AtTheMovies.Tests.Commands;

public class DeleteMovieCommandHandlerTests: IClassFixture<DbContextFixtureWithData>
{
    private readonly DbContextFixtureWithData _fixture;
    private readonly DeleteMovieCommandHandler _handler;

    public DeleteMovieCommandHandlerTests(DbContextFixtureWithData fixture)
    {
        _fixture = fixture;
        _handler = new DeleteMovieCommandHandler(_fixture.Context);
    }

    [Fact]
    public async Task Handle_ExistingMovie_ShouldDeleteSuccessfully()
    {
        // Arrange
        var existingMovie = _fixture.Context.Movies.First();
        existingMovie.Should().NotBeNull();

        var command = new DeleteMovieCommand { Id = existingMovie.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        var deletedMovie = await _fixture.Context.Movies.FindAsync(existingMovie.Id);
        deletedMovie.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NonExistingMovie_ShouldReturnFalse()
    {
        // Arrange
        var command = new DeleteMovieCommand { Id = 0 }; // Assuming 0 is an ID that does not exist in the database

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

    }

    [Fact]
    public async Task Handle_NonExistingMovie_ShouldNotAffectOtherMovies()
    {
        // Arrange
        var command = new DeleteMovieCommand { Id = 2 }; 
        var initialCount = _fixture.Context.Movies.Count();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        
        var finalCount = _fixture.Context.Movies.Count();
        finalCount.Should().Be(initialCount - 1);

        var otherMovie1 = await _fixture.Context.Movies.FindAsync(1);
        var otherMovie2 = await _fixture.Context.Movies.FindAsync(3);

        otherMovie1.Should().NotBeNull();
        otherMovie2.Should().NotBeNull();

    }
}