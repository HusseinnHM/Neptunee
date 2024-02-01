namespace Neptunee.Handlers.Requests;

public interface INeptuneeRequest
{
}

public interface INeptuneeRequest<TResponse> : INeptuneeRequest
{
}

public struct Void
{
    public static Void Value = new();
}