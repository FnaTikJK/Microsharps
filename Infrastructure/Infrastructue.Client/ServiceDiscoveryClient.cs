using ServiceDiscovery.API.Modules.RoutingModule.DTO;

namespace Infrastructure;

public interface IServiceDiscoveryClient
{
    Task<Result<T>> GetAsync<T>(string relativePath);
}

public class ServiceDiscoveryClient : IServiceDiscoveryClient
{
    private readonly string serviceName;
    private readonly string scheme;
    private readonly string serviceDiscoveryPath;
    
    private static readonly HttpClient httpClient = new();

    public ServiceDiscoveryClient(
        string serviceName, 
        string? serviceDiscoveryCustomHost, 
        string scheme = "http")
    {
        this.serviceName = serviceName;
        this.scheme = scheme;
        var serviceDiscoveryHost = serviceDiscoveryCustomHost
                                   ?? "localhost:8888";
        this.serviceDiscoveryPath = $"{scheme}://{serviceDiscoveryHost}/api/Routing";
    }
    
    public async Task<HttpResponseMessage> SendRequest(HttpRequestMessage requestMessage, CancellationToken? token)
    {
        return token != null
            ? await httpClient.SendAsync(requestMessage, token.Value)
            : await httpClient.SendAsync(requestMessage);
    }

    public async Task<Result<T>> GetAsync<T>(string relativePath)
    {
        var url = await BuildUrl(relativePath);
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return Result.ErrorFromHttp<T>(response);
        
        return Result.Ok(await response.Content.FromJsonOrThrow<T>());
    }

    private async Task<string> BuildUrl(string relativePath)
    {
        var serviceAddress = await ResolveServiceAddress();
        relativePath = relativePath.StartsWith('/')
            ? relativePath
            : $"/{relativePath}";

        return $"{scheme}://{serviceAddress}{relativePath}";
    }

    private async Task<string> ResolveServiceAddress()
    {
        var response = await httpClient.GetAsync(serviceDiscoveryPath + $"/{serviceName}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Can not resolve service host. ServiceName: {serviceName}");

        var deserialized = await response.Content.FromJsonOrThrow<GetServiceHostResponseModel>();
        return deserialized.Host;
    }
}