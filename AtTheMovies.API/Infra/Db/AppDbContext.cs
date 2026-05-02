using AtTheMovies.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.Infra.Db;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) 
{
    public DbSet<Movie> Movies => Set<Movie>();
}

