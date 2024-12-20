using System.Net.Http.Json;
using AbstractTaskService.Models.Request;
using Infrastructure;
using Infrastructure.Client;

namespace AbstractTaskService.Client;

public interface IAbstractTaskServiceClient
{
    Task<Result<string>> TestGet();
    Task<Result<TestPostResponseModel>> TestPost(TestPostRequestModel request);
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
        var response = await sdClient.GetAsync("api/Test/Test");
        if (!response.IsSuccess)
            return Result.ErrorFromHttp<string>(response);

        return Result.Ok(await response.Value.Content.ReadAsStringAsync());
    }

    public async Task<Result<TestPostResponseModel>> TestPost(TestPostRequestModel request)
    {
        var response = await sdClient.PostAsync("api/Test/TestPost", JsonContent.Create(request));
        if (!response.IsSuccess)
            return Result.ErrorFromHttp<TestPostResponseModel>(response);

        var responseModel = await response.Value.Content.FromJsonOrThrow<TestPostResponseModel>();
        return Result.Ok(responseModel);
    }
}