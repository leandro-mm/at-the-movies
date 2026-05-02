using MediatR;

namespace AtTheMovies.Commands.Movies;

public class UpdateMovieCommand: IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }="";
    public string Description { get; set; } = "";
    public string Genre { get; set; } = "";        
}