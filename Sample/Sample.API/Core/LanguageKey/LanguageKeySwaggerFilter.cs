using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Sample.SharedKernel;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample.API.Core.LanguageKey;

public class LanguageKeySwaggerFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = ConstValues.HeaderKeys.Language,
            In = ParameterLocation.Header,
            Required = false,
            AllowEmptyValue = false,
            Schema = new OpenApiSchema() { Type = "string" },
            Example = new OpenApiString("en"),
        });
    }
}