using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.Dispatchers.DomainEventDispatcher;
using Neptunee.DomainEvents.Dispatcher;
using Neptunee.DomainEvents.Handler;

namespace Neptunee.DependencyInjection;

public static class NeptuneeDomainEventsServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeDomainEvents(this IServiceCollection services, params Assembly[] assemblies) =>
        services
            .AddNeptuneeDomainEventsDispatcher()
            .AddNeptuneeDomainEventsHandlers(assemblies);

    public static IServiceCollection AddNeptuneeDomainEventsHandlers(this IServiceCollection services, params Assembly[] assemblies) =>
        services
            .Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableToAny(typeof(INeptuneeDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
    
    internal static IServiceCollection AddNeptuneeDomainEventsDispatcher(this IServiceCollection services) =>
        services
            .AddTransient<NeptuneeServiceFactory>(p => p.GetRequiredService)
            .AddTransient<INeptuneeDomainEventDispatcher, NeptuneeDomainEventDispatcher>();

    
}