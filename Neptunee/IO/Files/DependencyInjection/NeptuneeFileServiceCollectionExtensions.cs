using Microsoft.Extensions.DependencyInjection;
using Neptunee.IO.Files;
using Neptunee.IO.Files.Options;

namespace Neptunee.DependencyInjection;

public static class NeptuneeFileServiceCollectionExtensions
{
    public static IServiceCollection AddNeptuneeFileService(this IServiceCollection service, Action<NeptuneeFileOptions> option)
    {
        service.Configure(option).AddTransient<INeptuneeFileService, NeptuneeFileService>();
        return service;
    }
}