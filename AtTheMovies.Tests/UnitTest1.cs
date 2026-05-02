using MediatR;

namespace AtTheMovies.Tests;

public class UnitTest1
{
    [Fact]
    public void Ensure_CreateMovieCommand_Exists()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var handlerTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        foreach (var handler in handlerTypes)
        {
            Console.WriteLine($"Found handler: {handler.FullName}");
        }
    }
}
