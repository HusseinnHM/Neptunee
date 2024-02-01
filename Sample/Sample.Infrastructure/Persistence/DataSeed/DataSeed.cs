using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Application.Core.Abstractions.Security;
using Sample.Domain.Entities;
using Sample.Infrastructure.Persistence.Context;

namespace Sample.Infrastructure.Persistence.DataSeed;

public class DataSeed
{
    public static async Task Seed(SampleDbContext context, IServiceProvider serviceProvider)
    {
        await SeedUsers(context, serviceProvider.GetRequiredService<IPasswordHasher>());
    }

    private static async Task SeedUsers(SampleDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.ParticipationUsers.Any())
        {
            return;
        }

        context.ParticipationUsers.Add(new ParticipationUser("Seed participation", "seed@test.com", passwordHasher.HashPassword("1234")));
        context.EventManagers.Add(new EventManager("Seed EventManager", "seed@test.com", passwordHasher.HashPassword("1234")));

        await context.SaveChangesAsync();
    }
}