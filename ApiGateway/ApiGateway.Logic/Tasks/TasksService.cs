using AbstractTaskService.Client;
using AbstractTaskService.Models.Request;
using AbstractTaskService.Models.Response;
using Infrastructure;

namespace ApiGateway.Logic.Tasks;


public interface ITaskService
{
    Task<Result<GetTaskResponseModel>> GetTask(Guid id);
    Task<Result<AddTaskResponseModel>> AddTask(AddTaskRequestModel request);
    Task<Result<RetryTaskResponseModel>> RetryTask(RetryTaskRequestModel request);
}

public class TasksService : ITaskService
{
    private readonly IAbstractTaskServiceClient client;

    public TasksService(IAbstractTaskServiceClient client)
    {
        this.client = client;
    }

    public async Task<Result<GetTaskResponseModel>> GetTask(Guid id)
        => await client.GetTask(id);


    public async Task<Result<AddTaskResponseModel>> AddTask(AddTaskRequestModel request)
        => await client.AddTask(request);

    public async Task<Result<RetryTaskResponseModel>> RetryTask(RetryTaskRequestModel request)
        => await client.RetryTask(request);
}