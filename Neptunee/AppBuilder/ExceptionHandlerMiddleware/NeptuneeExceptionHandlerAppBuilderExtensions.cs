using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Neptunee.Extensions;

namespace Neptunee.AppBuilder.ExceptionHandlerMiddleware;

public static class NeptuneeExceptionHandlerAppBuilderExtensions
{
    public static IApplicationBuilder UseNeptuneeExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseExceptionHandler(cfg =>
        {
            cfg.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>()!;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(exceptionHandlerFeature.Error.ProblemDetails());
            });
        });
    }
}