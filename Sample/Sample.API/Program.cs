using Neptunee.AppBuilder.ExceptionHandlerMiddleware;
using Neptunee.DependencyInjection;
using Neptunee.EntityFrameworkCore.Extensions;
using Neptunee.OperationResponse.DependencyInjection;
using Neptunee.Swagger;
using Neptunee.Swagger.DependencyInjection;
using Sample.API;
using Sample.API.Core.LanguageKey;
using Sample.Application;
using Sample.Infrastructure;
using Sample.Infrastructure.Persistence.Context;
using Sample.Infrastructure.Persistence.DataSeed;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services
    .AddOperationSerializerOptions()
    .AddNeptuneeRequestDispatcher()
    .AddNeptuneeSwagger(o =>
    {
        o.AddJwtBearerSecurityScheme();
        o.SwaggerDocs<ApiGroupNames>();
        o.GroupNamesDocInclusion(ApiGroupNames.All.ToString());
        o.OperationFilter<LanguageKeySwaggerFilter>();
    })
    .AddNeptuneeFileService(o => o.SetBasePath(builder.Environment.WebRootPath))
    //.AddNeptuneeExceptionHandlerFilter()
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment)
    .AddNeptunee();

var app = builder.Build();

app
    .UseNeptuneeExceptionHandler()
    .UseNeptuneeSwagger(o => o.AddEndpoints<ApiGroupNames>().SetDocExpansion())
    .UseAuthorization();

app.MapControllers();

await app.MigrationAsync<SampleDbContext>(DataSeed.Seed);

app.Run();