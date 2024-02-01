using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.Handlers.RequestDispatcher;
using Neptunee.Handlers.Requests;
using Neptunee.Handlers.ServiceFactoryResolver;

namespace Neptunee.DependencyInjection;

public static class NeptuneeHandlersServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeRequestDispatcher(this IServiceCollection services) =>
        services.AddTransient<NeptuneeServiceFactory>(p => p.GetRequiredService)
            .AddTransient<INeptuneeRequestDispatcher, NeptuneeRequestDispatcher>();


    public static IServiceCollection AddNeptuneeRequestHandlers(this IServiceCollection services, params Assembly[] assemblies) =>
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableToAny(typeof(INeptuneeRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
}