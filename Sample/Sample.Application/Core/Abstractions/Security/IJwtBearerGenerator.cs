using System.Security.Claims;

namespace Sample.Application.Core.Abstractions.Security;

public interface IJwtBearerGenerator
{
    string Generate(List<Claim> claims);

}