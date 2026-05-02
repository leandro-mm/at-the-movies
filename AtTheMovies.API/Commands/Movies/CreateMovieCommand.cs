using MediatR;

namespace AtTheMovies.Commands.Movies;

public class CreateMovieCommand : IRequest<int>
{
    public string Name { get; set; }="";
    public string Description { get; set; } = "";
    public string Genre { get; set; } = "";
}