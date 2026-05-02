using AtTheMovies.DTOs.Movies;
using MediatR;

namespace AtTheMovies.Queries.Movies;

public class GetMoviesQuery: IRequest<List<MovieResponseDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SearchTerm { get; set; }
}