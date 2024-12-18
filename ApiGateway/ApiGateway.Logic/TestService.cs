using AbstractTaskService.Client;
using Infrastructure;

namespace ApiGateway.Logic;

public interface ITestService
{
    Task<Result<string>> Test();
}

public class TestService : ITestService
{
    private readonly IAbstractTaskServiceClient taskServiceClient;

    public TestService(IAbstractTaskServiceClient taskServiceClient)
    {
        this.taskServiceClient = taskServiceClient;
    }
    
    public async Task<Result<string>> Test()
    {
        return await taskServiceClient.TestGet();
    }
}