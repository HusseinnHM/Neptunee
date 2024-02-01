using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neptunee.Clock;
using Neptunee.DependencyInjection;
using Neptunee.DomainEvents.Dispatcher;
using Neptunee.EntityFrameworkCore.Interceptors;
using Neptunee.EntityFrameworkCore.MultiLanguage.DependencyInjection;
using Sample.Application.Core.Abstractions;
using Sample.Application.Core.Abstractions.Data;
using Sample.Infrastructure.Email;
using Sample.Infrastructure.Email.Settings;
using Sample.Infrastructure.FakeTranslate;
using Sample.Infrastructure.HttpResolver;
using Sample.Infrastructure.Persistence.Context;
using Sample.Infrastructure.Security.DependencyInjection;

namespace Sample.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,IWebHostEnvironment environment)
    {
 
        return services
            .AddDbContext<ISampleDbContext, SampleDbContext>((sp,o) =>
            {
                o.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                if (!environment.IsProduction())
                {
                    o.EnableSensitiveDataLogging();
                }

                o.AddInterceptors(new PublishNeptuneeDomainEventsInterceptor<Guid>(sp.GetRequiredService<INeptuneeDomainEventDispatcher>()));
                o.AddInterceptors(new UpdateAuditableNeptuneeEntitiesInterceptor<Guid>(sp.GetRequiredService<INeptuneeClock>()));
            })
            .AddMultiLanguage<SampleDbContext>()
            .AddNeptuneeRepositories(AssemblyReference.Assembly)
            .AddNeptuneeDomainEvents(Application.AssemblyReference.Assembly)
            .AddHttpContextAccessor()
            .Configure<MailSettings>(configuration.GetSection(MailSettings.SettingsKey))
            .AddScoped<IEmailService, EmailService>()
            .AddTransient<IHttpResolver,HttpResolverImp>()
            .AddTransient<IFakeTranslate,FakeTranslateImp>()
            .AddSecurity(configuration);
    }
}