using System.Net.Http.Json;
using AbstractTaskService.Models.Request;
using AbstractTaskService.Models.Response;
using Infrastructure;
using Infrastructure.Client;

namespace AbstractTaskService.Client;

public interface IAbstractTaskServiceClient
{
    Task<Result<AddTaskResponseModel>> AddTask(AddTaskRequestModel request);

    Task<Result<GetTaskResponseModel>> GetTask(Guid id);
    Task<Result<RetryTaskResponseModel>> RetryTask(RetryTaskRequestModel request);
}

public class AbstractTaskServiceClient : IAbstractTaskServiceClient
{
    private const string serviceName = "abstract-task-service"; // TODO: move service name in config/etc.
    private readonly IServiceDiscoveryClient sdClient;

    public AbstractTaskServiceClient()
    {
        sdClient = new ServiceDiscoveryClient(serviceName, null); // TODO: config
    }
    public async Task<Result<AddTaskResponseModel>> AddTask(AddTaskRequestModel request)
    {
        var response = await sdClient.PostAsync("api/Tasks/AddTask", JsonContent.Create(request));
        if (!response.IsSuccess)
            return Result.ErrorFromHttp<AddTaskResponseModel>(response);

        return Result.Ok(await response.Value.Content.FromJsonOrThrow<AddTaskResponseModel>());
    }

    public async Task<Result<GetTaskResponseModel>> GetTask(Guid id)
    {
        var response = await sdClient.GetAsync($"api/Tasks/{id}");
        if (!response.IsSuccess)
            return Result.ErrorFromHttp<GetTaskResponseModel>(response);

        return Result.Ok(await response.Value.Content.FromJsonOrThrow<GetTaskResponseModel>());
    }

    public async Task<Result<RetryTaskResponseModel>> RetryTask(RetryTaskRequestModel request)
    {
        var response = await sdClient.PostAsync("api/Tasks/RetryTask", JsonContent.Create(request));
        if (!response.IsSuccess)
            return Result.ErrorFromHttp<RetryTaskResponseModel>(response);

        return Result.Ok(await response.Value.Content.FromJsonOrThrow<RetryTaskResponseModel>());
    }
}