using AtTheMovies.DTOs.Movies;
using AtTheMovies.Handlers.Movies;
using AtTheMovies.Queries.Movies;
using AtTheMovies.Tests.Fixtures;
using FluentAssertions;

namespace AtTheMovies.Tests.Queries;

public class GetMoviesQueryHandlerTests : IClassFixture<DbContextFixtureWithData>
{
    private readonly DbContextFixtureWithData _fixture;
    private readonly GetMoviesQueryHandler _handler;

    public GetMoviesQueryHandlerTests(DbContextFixtureWithData fixture)
    {
        _fixture = fixture;
        _handler = new GetMoviesQueryHandler(_fixture.Context);
    }

    [Fact]
    public async Task Handle_NoFilters_ShouldReturnAllMovies()
    {
        // Arrange
        var expectedCount = _fixture.Context.Movies.Count();
        var query = new GetMoviesQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(expectedCount);
        result.Should().AllBeOfType<MovieResponseDto>();
    }

    [Fact]
    public async Task Handle_WithFilters_ShouldReturnFilteredMovies()
    {
        // Arrange        
        var query = new GetMoviesQuery
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = "Movie 1"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().AllBeOfType<MovieResponseDto>();
        result.First().Name.Should().Be(query.SearchTerm);
    }

     [Fact]
    public async Task Handle_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange        
        var query = new GetMoviesQuery
        {
            Page = 2, //segunda página com 1 item por página
            PageSize = 1
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().AllBeOfType<MovieResponseDto>();
        result.First().Name.Should().Be("Movie 2");
    }

    [Fact]
    public async Task Handle_WithSearchItemNotFound_ShouldReturnEmptyResult()
    {
        // Arrange        
        var query = new GetMoviesQuery
        {
            Page = 1,
            PageSize = 3,
            SearchTerm = "Non-existent Movie"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}