namespace Neptunee.Handlers.ServiceFactoryResolver;

public delegate object NeptuneeServiceFactory(Type serviceType);

internal static class NeptuneeServiceFactoryExtensions
{
    internal static T Resolve<T>(this NeptuneeServiceFactory factory)
        => (T)factory(typeof(T));
}