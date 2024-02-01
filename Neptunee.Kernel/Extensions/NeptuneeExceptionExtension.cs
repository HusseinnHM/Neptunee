using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Neptunee.Extensions;

public static class ExceptionExtensions
{
    public static string FullMessage(this Exception exception)
    {
        var fullMessage = new StringBuilder();
        return Recursive(exception);

        string Recursive(Exception ex)
        {
            fullMessage.Append(Environment.NewLine + ex + Environment.NewLine + ex.Message);
            if (ex.InnerException is null) return fullMessage.ToString();
            return Recursive(ex.InnerException);
        }
    }

    public static ProblemDetails ProblemDetails(this Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Detail = exception.Message,
            Type = exception.GetType().Name,
        };
        problemDetails.Extensions.Add("FullDetail", exception.FullMessage());
        problemDetails.Extensions.Add(nameof(exception.Source), exception.Source);
        problemDetails.Extensions.Add(nameof(exception.Data), exception.Data);
        return problemDetails;
    }
}