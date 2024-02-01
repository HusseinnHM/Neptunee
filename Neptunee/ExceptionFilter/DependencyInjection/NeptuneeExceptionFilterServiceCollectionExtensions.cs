using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Neptunee.Filters;

namespace Neptunee.DependencyInjection;

public static class NeptuneeExceptionFilterServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeExceptionHandlerFilter(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(o => o.Filters.Add<NeptuneeExceptionHandlerFilter>());
        return services;
    }
}