using AtTheMovies.DTOs.Movies;
using MediatR;

namespace AtTheMovies.Queries.Movies;

public class GetMovieByIdQuery: IRequest<MovieResponseDto?>
{
    public int Id { get; set; }
}