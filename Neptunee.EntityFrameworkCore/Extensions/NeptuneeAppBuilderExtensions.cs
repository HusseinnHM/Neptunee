using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Neptunee.EntityFrameworkCore.Extensions;

public static class NeptuneeAppBuilderExtensions
{
    public static async Task<IApplicationBuilder> MigrationAsync<TContext>(this IApplicationBuilder builder) where TContext : DbContext
    {
        var serviceScopeFactory = builder.ServiceScopeFactory();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.MigrateAsync();
        logger.LogInformation("Migrate is done");


        return builder;
    }

    public static async Task<IApplicationBuilder> MigrationAsync<TContext>(this IApplicationBuilder builder,
        Func<TContext, IServiceProvider, Task> seed) where TContext : DbContext
    {
        var serviceScopeFactory = builder.ServiceScopeFactory();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.MigrateAsync();
        logger.LogInformation("Migrate is done");

        await seed(context, serviceScope.ServiceProvider);
        logger.LogInformation("Seed is done");


        return builder;
    }

    public static async Task<IApplicationBuilder> MigrationAsync<TContext>(this IApplicationBuilder builder,
        Action<TContext, IServiceProvider> seed) where TContext : DbContext
    {
        var serviceScopeFactory = builder.ServiceScopeFactory();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        await context.Database.MigrateAsync();
        logger.LogInformation("Migrate is done");

        seed(context, serviceScope.ServiceProvider);
        logger.LogInformation("Seed is done");

        return builder;
    }

    public static async Task<IApplicationBuilder> SeedAsync<TContext>(this IApplicationBuilder builder,
        Func<TContext, IServiceProvider, Task> seed) where TContext : DbContext
    {
        var serviceScopeFactory = builder.ServiceScopeFactory();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        await seed(context, serviceScope.ServiceProvider);
        logger.LogInformation("Seed is done");

        return builder;
    }

    public static IApplicationBuilder Seed<TContext>(this IApplicationBuilder builder,
        Action<TContext, IServiceProvider> seed) where TContext : DbContext
    {
        var serviceScopeFactory = builder.ServiceScopeFactory();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

        seed(context, serviceScope.ServiceProvider);
        logger.LogInformation("Seed is done");

        return builder;
    }


    private static IServiceScopeFactory ServiceScopeFactory(this IApplicationBuilder builder) =>
        builder
            .ApplicationServices
            .GetRequiredService<IServiceScopeFactory>();
}