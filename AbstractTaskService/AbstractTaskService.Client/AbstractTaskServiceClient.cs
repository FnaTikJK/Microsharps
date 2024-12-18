using Infrastructure;

namespace AbstractTaskService.Client;

public interface IAbstractTaskServiceClient
{
    Task<Result<string>> TestGet();
}

public class AbstractTaskServiceClient : IAbstractTaskServiceClient
{
    private const string serviceName = "abstract-task-service"; // TODO: move service name in config/etc.
    private readonly IServiceDiscoveryClient sdClient;

    public AbstractTaskServiceClient()
    {
        sdClient = new ServiceDiscoveryClient(serviceName, null, "http"); // TODO: config
    }
    
    public async Task<Result<string>> TestGet()
    {
        return await sdClient.GetAsync<string>("api/Test/Test");
    }
}