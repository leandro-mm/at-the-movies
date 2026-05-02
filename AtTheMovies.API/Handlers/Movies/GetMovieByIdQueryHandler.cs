using AtTheMovies.DTOs.Movies;
using AtTheMovies.Infra.Db;
using AtTheMovies.Queries.Movies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.Handlers.Movies;

public class GetMovieByIdQueryHandler
    : IRequestHandler<GetMovieByIdQuery, MovieResponseDto?>
{
    private readonly AppDbContext _dbContext;
    
    public GetMovieByIdQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<MovieResponseDto?> Handle(
        GetMovieByIdQuery request
        , CancellationToken cancellationToken)
    {
        var movie = await _dbContext
                            .Movies
                            .AsNoTracking()
                            .Where(x => x.Id == request.Id)
                            .Select(m => new MovieResponseDto
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Description = m.Description,
                                Genre = m.Genre,
                                CreatedAt = m.CreatedAt,
                                LastUpdate = m.LastUpdate
                            })
                            .FirstOrDefaultAsync(cancellationToken);

        return movie;
    }
}