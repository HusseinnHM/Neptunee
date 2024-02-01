using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Sample.Infrastructure.Security.Policies.UserType;

namespace Sample.Infrastructure.Security.Policies;

public class SampleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(HasUserTypesAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var @operator = HasUserTypesAttribute.GetOperatorFromPolicy(policyName);
            var userTypes = HasUserTypesAttribute.GetUserTypesFromPolicy(policyName);

            var requirement = new UserTypeRequirement(@operator, userTypes);

            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}