using ServiceDiscovery.Models.Requests;

namespace Infrastructure.Client;

public interface IServiceDiscoveryClient
{
    Task<Result<HttpResponseMessage>> GetAsync(string? relativePath);
    Task<Result<HttpResponseMessage>> PostAsync(string? relativePath, HttpContent? content);
    Task<Result<HttpResponseMessage>> PatchAsync(string? relativePath, HttpContent? content);
    Task<Result<HttpResponseMessage>> DeleteAsync(string? relativePath, HttpContent? content);
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

    public async Task<Result<HttpResponseMessage>> GetAsync(string? relativePath)
    {
        var url = await BuildUrl(relativePath);
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return Result.ErrorFromHttp<HttpResponseMessage>(response);
        
        return Result.Ok(response);
    }

    public async Task<Result<HttpResponseMessage>> PostAsync(string? relativePath, HttpContent? content)
    {
        var url = await BuildUrl(relativePath);
        var response = await httpClient.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
            return Result.ErrorFromHttp<HttpResponseMessage>(response);
        
        return Result.Ok(response);
    }
    
    public async Task<Result<HttpResponseMessage>> PatchAsync(string? relativePath, HttpContent? content)
    {
        var url = await BuildUrl(relativePath);
        var response = await httpClient.PatchAsync(url, content);
        if (!response.IsSuccessStatusCode)
            return Result.ErrorFromHttp<HttpResponseMessage>(response);
        
        return Result.Ok(response);
    }
    
    public async Task<Result<HttpResponseMessage>> DeleteAsync(string? relativePath, HttpContent? content)
    {
        var url = await BuildUrl(relativePath);
        var requestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Content = content,
            Method = HttpMethod.Delete,
        };
        var response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            return Result.ErrorFromHttp<HttpResponseMessage>(response);
        
        return Result.Ok(response);
    }

    private async Task<string> BuildUrl(string? relativePath)
    {
        var serviceAddress = await ResolveServiceAddress();
        relativePath ??= "";
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