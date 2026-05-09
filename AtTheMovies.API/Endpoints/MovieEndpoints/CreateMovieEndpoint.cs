using AtTheMovies.Commands.Movies;
using MediatR;

namespace AtTheMovies.API.Endpoints.MovieEndpoints;

public static class CreateMovieEndpoint
{
    public static void MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
        IMediator mediator,
        CreateMovieCommand command,
        CancellationToken ct) =>
        {
            var movieId = await mediator.Send(command, ct);
            return Results.Created($"/{movieId}", new { Id = movieId });
        })
        .WithDisplayName("Create Movie")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }
}