using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Neptunee.Extensions;

namespace Neptunee.Filters;

public class NeptuneeExceptionHandlerFilter : IAsyncExceptionFilter
{
    private readonly ILogger<NeptuneeExceptionHandlerFilter> _logger;

    public NeptuneeExceptionHandlerFilter(ILogger<NeptuneeExceptionHandlerFilter> logger)
    {
        _logger = logger;
    }


    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!context.ExceptionHandled)
        {
            _logger.LogError(context.Exception, "An unhandled exception has occurred while executing the request.");
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.HttpContext.Response.WriteAsJsonAsync(context.Exception.ProblemDetails());
            context.ExceptionHandled = true;
        }
    }
}