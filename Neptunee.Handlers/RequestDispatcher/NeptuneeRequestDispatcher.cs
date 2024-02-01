using Neptunee.Handlers.Requests;
using Neptunee.Handlers.ServiceFactoryResolver;

namespace Neptunee.Handlers.RequestDispatcher;

public sealed class NeptuneeRequestDispatcher : INeptuneeRequestDispatcher
{
    private readonly NeptuneeServiceFactory _serviceFactory;

    public NeptuneeRequestDispatcher(NeptuneeServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }


    public async Task<TResponse> SendAsync<TRequest,TResponse>(TRequest request,CancellationToken cancellationToken = default) where TRequest : class, INeptuneeRequest<TResponse>
    {
        var handler = _serviceFactory.Resolve<INeptuneeRequestHandler<TRequest,TResponse>>();
        return await handler.HandleAsync(request, cancellationToken);
    }
}