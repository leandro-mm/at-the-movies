using AtTheMovies.Queries.Movies;
using MediatR;

namespace AtTheMovies.API.Endpoints.MovieEndpoints;

public static class GetMovieByIdEndpoint
{
    public static void MapGetMovieById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", async (
            IMediator mediator,
            int id,
            CancellationToken ct) =>
        {
            var query = new GetMovieByIdQuery { Id = id };
            var movie = await mediator.Send(query, ct);
            return movie is null ? Results.NotFound() : Results.Ok(movie);
        })
        .WithDisplayName("Get Movie By Id")
        .Produces(StatusCodes.Status200OK)        
        .ProducesValidationProblem(StatusCodes.Status404NotFound);
    }
}