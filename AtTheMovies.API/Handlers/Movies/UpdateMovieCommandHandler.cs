using AtTheMovies.Commands.Movies;
using AtTheMovies.Infra.Db;
using MediatR;

namespace AtTheMovies.Handlers.Movies;

public class UpdateMovieCommandHandler
    : IRequestHandler<UpdateMovieCommand, bool>
{
    private readonly AppDbContext _dbContext;

    public UpdateMovieCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(
        UpdateMovieCommand request
        , CancellationToken cancellationToken)
    {
        var movie = await _dbContext
                     .Movies
                     .FindAsync(new object[] { request.Id }, cancellationToken);

        if (movie is null)
            return false;

        movie.Name = request.Name;
        movie.Description = request.Description;
        movie.Genre = request.Genre;
        movie.LastUpdate = DateTime.UtcNow;

        //_dbContext.Movies.Update(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}