using Neptunee.Swagger.Attributes;

namespace Sample.API;

public class ApiGroupAttribute : NeptuneeApiGroupAttribute<ApiGroupNames>
{
    public ApiGroupAttribute(params ApiGroupNames[] param) : base(param)
    {
    }
}