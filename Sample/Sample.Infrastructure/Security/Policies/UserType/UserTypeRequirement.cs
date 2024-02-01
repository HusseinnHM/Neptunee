using Microsoft.AspNetCore.Authorization;
using Sample.SharedKernel;

namespace Sample.Infrastructure.Security.Policies.UserType;

public class UserTypeRequirement : IAuthorizationRequirement
{
    public static string ClaimType => ConstValues.ClaimTypes.UserType;
        
    public Operator Operator { get; }
        
    public string[] UserTypes { get; }

    public UserTypeRequirement(Operator @operator, string[] userTypes)
    {
        if (userTypes.Length == 0)
            throw new ArgumentException("At least one UserType is required.", nameof(userTypes));

        Operator = @operator;
        UserTypes = userTypes;
    }
}