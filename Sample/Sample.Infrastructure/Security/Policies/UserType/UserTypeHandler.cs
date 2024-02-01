using Microsoft.AspNetCore.Authorization;

namespace Sample.Infrastructure.Security.Policies.UserType;

public class UserTypeHandler : AuthorizationHandler<UserTypeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, UserTypeRequirement requirement)
    {
        if (requirement.Operator == Operator.And)
        {
            foreach (var userType in requirement.UserTypes)
            {
                if (!context.User.HasClaim(c => c.Type == UserTypeRequirement.ClaimType && c.Value.Equals(userType,StringComparison.OrdinalIgnoreCase)))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }
                
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        foreach (var userType in requirement.UserTypes)
        {
            if (context.User.HasClaim(c => c.Type == UserTypeRequirement.ClaimType && c.Value.Equals(userType,StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
                
        context.Fail();
        return Task.CompletedTask;
    }
}