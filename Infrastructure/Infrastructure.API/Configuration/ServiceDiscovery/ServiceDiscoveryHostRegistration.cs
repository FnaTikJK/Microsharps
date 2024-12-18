using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.API.Configuration.ApplicationBuilder;

public static class ServiceDiscoveryHostRegistration
{
    public static void RegisterServiceDiscovery(this IServiceCollection services, string serviceName)
    {
        services.AddSingleton<IServiceDiscoveryConfigurationClient, ServiceDiscoveryConfigurationClient>(
            s =>
            {
                var hosts = s.GetService<IServer>()!.Features.Get<IServerAddressesFeature>()!.Addresses;
                
                return new ServiceDiscoveryConfigurationClient(
                    hosts,
                    serviceName,
                    null,
                    "http");
            });
    }
}