using AtTheMovies.Commands.Movies;
using AtTheMovies.DTOs.Movies;
using AtTheMovies.Queries.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class MovieEndpoints
{
    const string MovieEndpointName = "GetMovies";
    public static RouteGroupBuilder MapMovieEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/movies");

        group.MapGet("/", async (
            IMediator mediator,
            [AsParameters] GetMoviesQuery query,
            CancellationToken ct) =>
        {
            var movies = await mediator.Send(query, ct);
            return Results.Ok(movies);
        })
        .WithName(MovieEndpointName)
        .WithDisplayName("Get Movies");

        group.MapPost("/", async (
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

        group.MapGet("/{id}", async (
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

        group.MapPut("/{id}", async (
            IMediator mediator,
            int id,
            [FromBody] UpdateMovieDto dto,
            CancellationToken ct) =>
        {
            var command = new UpdateMovieCommand
            {
                Id = id,  // Take ID from route, not from body
                Name = dto.Name,
                Description = dto.Description,
                Genre = dto.Genre
            };

            var result = await mediator.Send(command, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithDisplayName("Update Movie")
        .Produces(StatusCodes.Status204NoContent)        
        .ProducesValidationProblem(StatusCodes.Status404NotFound);


        group.MapDelete("/{id}", async (
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

        return group;
    }
}