using AtTheMovies.Commands.Movies;
using MediatR;

namespace AtTheMovies.API.Endpoints.MovieEndpoints;

public static class DeleteMovieEndpoint
{
    public static void MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", async (
            IMediator mediator, 
            int id,
            CancellationToken ct) =>
        {
            var command = new DeleteMovieCommand { Id = id };
            var result = await mediator.Send(command, ct); 
            return result ? Results.Ok() : Results.NotFound();
        }) 
        .WithDisplayName("Delete Movie")
        .Produces(StatusCodes.Status200OK)        
        .ProducesValidationProblem(StatusCodes.Status404NotFound);
    }
}