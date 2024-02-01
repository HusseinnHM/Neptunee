using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Sample.Application.Core.Abstractions;
using Sample.Infrastructure.HttpResolver.Exceptions;
using Sample.SharedKernel;

namespace Sample.Infrastructure.HttpResolver;

public class HttpResolverImp : IHttpResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HttpResolverImp(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CurrentUserId()
    {
        return Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : Guid.Empty;
    }

    public string LanguageKey()
    {
        var languageKey = _httpContextAccessor.HttpContext?.Request.Headers[ConstValues.HeaderKeys.Language].ToString() ?? string.Empty;

        return LanguageKeys.Validate(languageKey) || true
            ? languageKey
            : throw new InvalidHeaderException(ConstValues.HeaderKeys.Language);
    }

    public bool IsDefaultLanguageKey()
    {
        return LanguageKeys.Default.Equals(LanguageKey(), StringComparison.OrdinalIgnoreCase);
    }
}