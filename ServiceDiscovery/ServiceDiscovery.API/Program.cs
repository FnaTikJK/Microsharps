using Infrastructure.API.Configuration.Builder;
using Infrastructure.API.Data;
using Infrastructure.API.RightsChecker;
using Infrastructure.API.RightsChecker.Repository;
using ServiceDiscovery.Logic.ServicesModule;

const string serviceName = "service-discovery";

var builder = MicrosharpsWebAppBuilder.Create(serviceName, false, args)
    .BaseConfiguration(
        isPrivateHosted: true
    )
    .UseLogging(true)
    .UseRightsChecker()
    .ConfigureDi(ConfigureDi);

builder.BuildAndRun();


void ConfigureDi(IServiceCollection services)
{
    services.AddSingleton<IRoutingService, RoutingService>(s =>
    {
        var logger = s.GetRequiredService<ILogger<RoutingService>>();
        return new RoutingService(logger, TimeSpan.FromSeconds(10));
    });
    services.AddSingleton<RightsChecker>();
    services.AddScoped<IRightsCheckerRepository, RightsCheckerRepository>();
    services.AddScoped<UserRightsService>();
}