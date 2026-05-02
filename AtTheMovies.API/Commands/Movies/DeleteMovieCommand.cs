using MediatR;

namespace AtTheMovies.Commands.Movies;

public class DeleteMovieCommand: IRequest<bool>
{
    public int Id { get; set; }
}