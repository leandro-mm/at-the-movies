using AtTheMovies.Queries.Movies;
using MediatR;

namespace AtTheMovies.API.Endpoints.MovieEndpoints;

public static class GetMoviesEndpoint
{
     const string MovieEndpointName = "GetMovies";
     
    public static void MapGetMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            IMediator mediator,
            [AsParameters] GetMoviesQuery query,
            CancellationToken ct) =>
        {
            var movies = await mediator.Send(query, ct);
            return Results.Ok(movies);
        })
        .WithName(MovieEndpointName)
        .WithDisplayName("Get Movies");
    }
}