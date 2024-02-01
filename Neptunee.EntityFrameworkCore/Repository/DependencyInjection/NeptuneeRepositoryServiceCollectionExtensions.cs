using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.EntityFrameworkCore.Repository;

namespace Neptunee.DependencyInjection;

public static class NeptuneeRepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeRepositories(this IServiceCollection services, params Assembly[] assemblies) =>
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableToAny(typeof(INeptuneeRepository), typeof(INeptuneeReadRepository)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
}