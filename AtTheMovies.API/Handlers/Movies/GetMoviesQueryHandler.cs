using AtTheMovies.DTOs.Movies;
using AtTheMovies.Infra.Db;
using AtTheMovies.Queries.Movies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.Handlers.Movies;

public class GetMoviesQueryHandler
    : IRequestHandler<GetMoviesQuery, List<MovieResponseDto>>
{
    private readonly AppDbContext _dbContext;

    public GetMoviesQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MovieResponseDto>> Handle(
        GetMoviesQuery request
        , CancellationToken cancellationToken)
    {
        var query = _dbContext.Movies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query
                    .Where(m =>
                        m.Name.Contains(request.SearchTerm) ||
                        m.Description.Contains(request.SearchTerm));
        }
        
        var movies = query
                    .OrderBy(m => m.Id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(m => new MovieResponseDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Genre = m.Genre,
                        LastUpdate = m.LastUpdate,
                        CreatedAt = m.CreatedAt
                    });

        var result = await movies.ToListAsync(cancellationToken);
        return result;
    }

}