using Microsoft.Extensions.DependencyInjection;

namespace Neptunee.DependencyInjection;

public static class NeptuneeCollectionExtensions
{
    public static IServiceCollection AddNeptunee(this IServiceCollection services) =>
        services.AddNeptuneeClock();
}