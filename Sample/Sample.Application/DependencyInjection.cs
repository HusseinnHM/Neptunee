using Microsoft.Extensions.DependencyInjection;
using Neptunee.DependencyInjection;

namespace Sample.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddNeptuneeRequestHandlers(AssemblyReference.Assembly);
    }
}