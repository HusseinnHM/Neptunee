namespace Neptunee.Handlers.Requests;

public interface INeptuneeRequestHandler<in TRequest, TResponse> where TRequest : class, INeptuneeRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface INeptuneeRequestHandler<in TRequest> : INeptuneeRequestHandler<TRequest, Void> where TRequest : class, INeptuneeRequest<Void>
{
}

