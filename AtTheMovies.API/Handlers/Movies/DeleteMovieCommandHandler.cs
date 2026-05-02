using AtTheMovies.Commands.Movies;
using AtTheMovies.Infra.Db;
using MediatR;

namespace AtTheMovies.Handlers.Movies;

public class DeleteMovieCommandHandler
    : IRequestHandler<DeleteMovieCommand, bool>
{
     private readonly AppDbContext _dbContext;

     public DeleteMovieCommandHandler(AppDbContext dbContext)
     {
        _dbContext = dbContext;
     }
    public async Task<bool> Handle(
        DeleteMovieCommand request
        , CancellationToken cancellationToken)
    {
       var movies = await _dbContext
                    .Movies
                    .FindAsync(new object[] { request.Id }, cancellationToken);

        if (movies is null)
            return false;

        _dbContext.Movies.Remove(movies);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}