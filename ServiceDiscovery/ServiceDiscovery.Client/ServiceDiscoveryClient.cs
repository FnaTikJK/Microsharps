using System.Net.Http.Json;
using Infrastructure;
using ServiceDiscovery.API.Modules.RoutingModule.DTO;

namespace ServiceDiscovery.Client;

public interface IServiceDiscoveryClient
{
    Task<Result<string>> Register(string ownHost);
}

public class ServiceDiscoveryClient : IServiceDiscoveryClient
{
    private readonly string serviceDiscoveryHost;
    private readonly string serviceName;
    private readonly string scheme;
    private string basePath => $"{scheme}://{serviceDiscoveryHost}/api/Routing";
    
    private static readonly HttpClient httpClient = new();

    public ServiceDiscoveryClient(string serviceName, string? serviceDiscoveryHost, string scheme = "http")
    {
        this.serviceName = serviceName;
        this.serviceDiscoveryHost = serviceDiscoveryHost ?? "localhost:8888";
        this.scheme = scheme;
    }

    public async Task<Result<string>> Register(string ownHost)
    {
        var path = basePath;
        var content = JsonContent.Create(new { ServiceName = serviceName });
        ownHost = ownHost.Replace("http://", "").Replace("localhost", "127.0.0.1");
        content.Headers.Add("Sd-host", ownHost);
        var response = await httpClient.PostAsync(path, content);
        if (!response.IsSuccessStatusCode)
            return Result.BadRequest<string>(response.StatusCode.ToString());
        
        return Result.Ok("");
    }

    public async Task<HttpResponseMessage> SendRequest(HttpRequestMessage requestMessage, CancellationToken? token)
    {
        return token != null
            ? await httpClient.SendAsync(requestMessage, token.Value)
            : await httpClient.SendAsync(requestMessage);
    }

    private async Task<string> ResolveServiceAddress()
    {
        var serviceDiscoveryResponse = await httpClient.GetAsync(serviceDiscoveryHost + $"/api/Routing/{serviceName}");
        var a = serviceDiscoveryResponse.Content;
        throw new NotImplementedException();
    }
}