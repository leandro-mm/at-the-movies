using AtTheMovies.Infra.Db;
using Microsoft.EntityFrameworkCore;

namespace AtTheMovies.API.Extensions;

public static class AddSqLiteExtension
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddSqlite<AppDbContext>(connectionString);

        return services;
    }
}