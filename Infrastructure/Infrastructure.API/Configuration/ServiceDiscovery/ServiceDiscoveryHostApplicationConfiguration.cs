using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceDiscovery.API.Modules.RoutingModule.DTO;

namespace Infrastructure.API.Configuration.Application;

public static class ServiceDiscoveryHostApplicationConfiguration
{
    public static void ConfigureServiceDiscoveryLifetime(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var result = RegisterHosts(app).GetAwaiter().GetResult();
            if (!result.IsSuccess)
                throw new ApplicationConfigurationException($"Can not register service in SD. Error: {result.Error}");
            
            app.Logger.LogInformation("Success register service in SD");
        });

        app.Lifetime.ApplicationStopping.Register(() =>
        {
            var result = RemoveHosts(app).GetAwaiter().GetResult();
            app.Logger.LogInformation(!result.IsSuccess
                ? $"Can not remove service from SD. Error: {result.Error}"
                : "Success remove service from SD");
        });
    }

    private static async Task<Result<RegisterServiceResponseModel>> RegisterHosts(WebApplication app)
    {
        // TODO: Довести до ума(конфиги и тд)
        var configurationClient = app.Services.GetService<IServiceDiscoveryConfigurationClient>();
        if (configurationClient == null)
            return Result.BadRequest<RegisterServiceResponseModel>($"Can not resolve {nameof(IServiceDiscoveryConfigurationClient)} for register service in Service Discovery.");

        return await configurationClient.Register();
    }

    private static async Task<EmptyResult> RemoveHosts(WebApplication app)
    {
        var configurationClient = app.Services.GetService<IServiceDiscoveryConfigurationClient>();
        if (configurationClient == null)
            return EmptyResult.BadRequest($"Can not resolve {nameof(IServiceDiscoveryConfigurationClient)} for register service in Service Discovery.");

        return await configurationClient.Remove();
    }
}