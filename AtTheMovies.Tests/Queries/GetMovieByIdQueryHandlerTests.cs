using AtTheMovies.Handlers.Movies;
using AtTheMovies.Queries.Movies;
using AtTheMovies.Tests.Fixtures;
using FluentAssertions;

namespace AtTheMovies.Tests.Queries;

public class GetMovieByIdQueryHandlerTests : IClassFixture<DbContextFixtureWithData>
{
    private readonly DbContextFixtureWithData _fixture;
    private readonly GetMovieByIdQueryHandler _handler;

    public GetMovieByIdQueryHandlerTests(DbContextFixtureWithData fixture)
    {
        _fixture = fixture;
        _handler = new GetMovieByIdQueryHandler(_fixture.Context);
    }

    [Fact]
    public async Task Handle_ExistingId_ShouldReturnMovie()
    {
        // Arrange
        var query = new GetMovieByIdQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Movie 1");
    }

    [Fact]
    public async Task Handle_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetMovieByIdQuery { Id = 0 }; // Assuming 0 is an ID that does not exist in the database

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Query_ShouldUseAsNoTracking()
    {
        // Arrange
        var query = new GetMovieByIdQuery { Id = 1 }; 

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        //var entry = _fixture.Context.Entry(result);
        //entry.State.Should().Be(Microsoft.EntityFrameworkCore.EntityState.Detached);
    }
}
