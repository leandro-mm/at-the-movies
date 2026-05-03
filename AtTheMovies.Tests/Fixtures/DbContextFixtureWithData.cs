using AtTheMovies.Entities;
using AtTheMovies.Infra.Db;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.Tests.Fixtures;

public class DbContextFixtureWithData : IDisposable
{
    public AppDbContext Context { get; private set; }

    public DbContextFixtureWithData()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: string.Concat("TestDatabaseWithData-", Guid.NewGuid().ToString()))
            .Options;
        Context = new AppDbContext(options);
        SeedData();

    }

    public void SeedData()
    {
        var movies = new List<Movie>
        {
             new Movie { 
                Id = 1, Name = "Movie 1", 
                Genre = "Action", 
                CreatedAt = 
                new DateTime(2020, 1, 1) 
            },
             new Movie { 
                Id = 2, 
                Name = "Movie 2", 
                Genre = "Comedy", 
                CreatedAt = new DateTime(2021, 1, 1) 
            },
             new Movie { 
                Id = 3, 
                Name = "Movie 3", 
                Genre = "Drama", 
                CreatedAt = new DateTime(2022, 1, 1) 
            }
        };
        Context.Movies.AddRange(movies);
        Context.SaveChanges();
    }
    public void Dispose()
    {
        // Cleanup code here
    }
}