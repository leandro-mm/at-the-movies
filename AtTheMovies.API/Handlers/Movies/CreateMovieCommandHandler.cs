using AtTheMovies.Commands.Movies;
using AtTheMovies.Entities;
using AtTheMovies.Infra.Db;
using MediatR;

namespace AtTheMovies.Handlers.Movies;

public class CreateMovieCommandHandler 
    : IRequestHandler<CreateMovieCommand, int>
{
    private readonly AppDbContext _dbContext;

    public CreateMovieCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<int> Handle(
        CreateMovieCommand request
        , CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Name = request.Name,
            Description = request.Description,
            Genre = request.Genre,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return movie.Id;

    }
}