using Microsoft.AspNetCore.Authorization;

namespace Sample.Infrastructure.Security.Policies.UserType;

public class HasUserTypesAttribute : AuthorizeAttribute
{
    internal const string PolicyPrefix = "HasUserTypes_";
    private const string Separator = "_";

   
    public HasUserTypesAttribute(Operator @operator, params string[] userTypes)
    {
        Policy = $"{PolicyPrefix}{(int)@operator}{Separator}{string.Join(Separator, userTypes)}";
    }

 
    public HasUserTypesAttribute(string userType)
    {
        Policy = $"{PolicyPrefix}{(int)Operator.And}{Separator}{userType}";
    }

    public static Operator GetOperatorFromPolicy(string policyName)
    {
        var @operator = int.Parse(policyName.AsSpan(PolicyPrefix.Length, 1));
        return (Operator)@operator;
    }

    public static string[] GetUserTypesFromPolicy(string policyName)
    {
        return policyName.Substring(PolicyPrefix.Length + 2)
            .Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);
    }
}