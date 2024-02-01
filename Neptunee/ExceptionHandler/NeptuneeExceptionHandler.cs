using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Neptunee.Extensions;

namespace Neptunee.ExceptionHandler;

#if NET8_0
public class NeptuneeExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NeptuneeExceptionHandler> _logger;

    public NeptuneeExceptionHandler(ILogger<NeptuneeExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception has occurred while executing the request.");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(exception.ProblemDetails(), cancellationToken);
        return true;
    }
}
#endif