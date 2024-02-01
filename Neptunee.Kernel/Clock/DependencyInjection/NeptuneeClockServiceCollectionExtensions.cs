using Microsoft.Extensions.DependencyInjection;
using Neptunee.Clock;

namespace Neptunee.DependencyInjection;

public static class NeptuneeClockServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeClock(this IServiceCollection services) => 
        services.AddScoped<INeptuneeClock, NeptuneeClockImp>();
    
}