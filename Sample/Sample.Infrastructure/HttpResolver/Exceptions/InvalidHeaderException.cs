namespace Sample.Infrastructure.HttpResolver.Exceptions;

public class InvalidHeaderException : Exception
{
    public InvalidHeaderException(string headerKey) : base($"Invalid {headerKey} header")
    {
        
    }
}