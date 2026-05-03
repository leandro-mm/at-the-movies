using AtTheMovies.Infra.Db;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.Tests.Fixtures;

public class DbContextFixture: IDisposable
{
    public AppDbContext CreateDbContext()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;
    return new AppDbContext(options);

}
    public void Dispose()
    {
        // Cleanup code here
    }
}