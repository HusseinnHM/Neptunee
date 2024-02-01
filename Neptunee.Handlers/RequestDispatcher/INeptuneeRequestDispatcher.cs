using Neptunee.Handlers.Requests;

namespace Neptunee.Handlers.RequestDispatcher;

public interface INeptuneeRequestDispatcher
{
    Task<TResponse> SendAsync<TRequest,TResponse>(TRequest request,CancellationToken cancellationToken = default) where TRequest : class, INeptuneeRequest<TResponse>;
}