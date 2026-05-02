using AtTheMovies.Behaviors;
using FluentValidation;
using MediatR;

namespace AtTheMovies.API.Extensions;

public static class AddMediatRextension
{
    public static IServiceCollection AddMediatRExtension(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()
        ));

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);   
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}