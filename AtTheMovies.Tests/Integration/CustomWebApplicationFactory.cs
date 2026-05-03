using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AtTheMovies.Infra.Db;

namespace AtTheMovies.Tests.Integration;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveAllDatabaseRegistrations(services);
            AddInMemoryDatabase(services);
        });

        // builder.ConfigureAppConfiguration((context, config) =>
        // {
        //     // Optional: Add test-specific configuration
        //     // config.AddJsonFile("appsettings.Test.json", optional: true);
        //     // config.AddInMemoryCollection(new Dictionary<string, string>
        //     // {
        //     //     // Add test-specific settings
        //     //     // ["ConnectionStrings:DefaultConnection"] = "InMemoryTest"
        //     // });
        // });
    }

    private static void AddInMemoryDatabase(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
            options.EnableSensitiveDataLogging(); // Optional: for better debugging
        });

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
        SeedDatabase(dbContext);
    }

    private static void RemoveAllDatabaseRegistrations(IServiceCollection services)
    {
        RemoveDbContext(services);
        RemoveDbContextOptions(services);
        RemoveGenericDbContextOptions(services);
        RemoveIDbContextFactory(services);
        RemoveDatabaseProvider(services);
        RemoveDatabaseProviderOthers(services);
    }

    private static void RemoveDatabaseProviderOthers(IServiceCollection services)
    {
        var providerServices = services
            .Where(d => d.ServiceType.Namespace?.Contains("Microsoft.EntityFrameworkCore") == true)
            .ToList();

        foreach (var service in providerServices)
        {
            services.Remove(service);
        }
    }

    private static void RemoveDatabaseProvider(IServiceCollection services)
    {
        var sqliteOptionsDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptionsBuilder));
        if (sqliteOptionsDescriptor != null)
            services.Remove(sqliteOptionsDescriptor);
    }

    private static void RemoveIDbContextFactory(IServiceCollection services)
    {
        var factoryDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IDbContextFactory<AppDbContext>));
        if (factoryDescriptor != null)
            services.Remove(factoryDescriptor);
    }

    private static void RemoveGenericDbContextOptions(IServiceCollection services)
    {
        var genericOptionsDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions));
        if (genericOptionsDescriptor != null)
            services.Remove(genericOptionsDescriptor);
    }

    private static void RemoveDbContextOptions(IServiceCollection services)
    {
        var optionsDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (optionsDescriptor != null)
            services.Remove(optionsDescriptor);
    }

    private static void RemoveDbContext(IServiceCollection services)
    {
        var dbContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(AppDbContext));
        if (dbContextDescriptor != null)
            services.Remove(dbContextDescriptor);
    }

    private static void SeedDatabase(AppDbContext dbContext)
    {
        // Add any test seed data here if needed
        // Example:
        // dbContext.Movies.AddRange(GetTestMovies());
        // dbContext.SaveChanges();
    }
}