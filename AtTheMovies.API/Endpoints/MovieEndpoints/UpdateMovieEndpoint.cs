using AtTheMovies.Commands.Movies;
using AtTheMovies.DTOs.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AtTheMovies.API.Endpoints.MovieEndpoints;

public static class UpdateMovieEndpoint
{
    public static void MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", async (
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
    }
}