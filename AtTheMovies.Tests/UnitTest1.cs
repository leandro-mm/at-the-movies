using AtTheMovies.Commands.Movies;
using MediatR;

namespace AtTheMovies.Tests;

public class UnitTest1
{
    [Fact]
    public void CreateMovieCommand_Exists()
    {
        //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assemblyWithHandlers = typeof(CreateMovieCommand).Assembly;
        var handlerTypes = assemblyWithHandlers
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        var command = handlerTypes.FirstOrDefault(t => t.Name == "CreateMovieCommandHandler");
        
        // foreach (var handler in handlerTypes)
        // {
        //     Console.WriteLine($"Found handler: {handler.FullName}");
        // }

        Assert.NotNull(command);

        
    }
}
