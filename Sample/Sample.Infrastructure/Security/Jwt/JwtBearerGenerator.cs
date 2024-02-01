using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventManagement.Infrastructure.Jwt.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sample.Application.Core.Abstractions.Security;

namespace Sample.Infrastructure.Security.Jwt;

public class JwtBearerGenerator : IJwtBearerGenerator
{
    private readonly JwtOptions _jwtOptions;
        
    public JwtBearerGenerator(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }


    public string Generate(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.Add(_jwtOptions.Expire),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}