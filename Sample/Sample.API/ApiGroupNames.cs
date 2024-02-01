using Neptunee.Swagger.Attributes;

namespace Sample.API;

public enum ApiGroupNames
{
    [NeptuneeDocInfoGenerator(title: "All")]
    All,
    [NeptuneeDocInfoGenerator(title: "V1", version: "v1")]
    V1,

    [NeptuneeDocInfoGenerator(title: "V2", version: "v2")]
    V2,
}