namespace Sample.Application.Core.Abstractions;

public interface IHttpResolver
{
    Guid CurrentUserId();
    string LanguageKey();
    bool IsDefaultLanguageKey();
}