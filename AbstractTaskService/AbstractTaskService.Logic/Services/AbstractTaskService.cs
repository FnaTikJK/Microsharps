using System.Text;
using AbstractTaskService.DAL.Entities;
using AbstractTaskService.DAL.Repositories;
using AbstractTaskService.Logic.Requests;
using AbstractTaskService.Logic.Response;
using Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace AbstractTaskService.Logic.Services;

public interface IAbstractTaskService
{
    Task<Result<GetTaskResponse>> GetTask(GetTaskRequest request);
    Task<Result<AddTaskResponse>> AddTask(AddTaskRequest request);
    Task<Result<RetryTaskResponse>> RetryTask(RetryTaskRequest request);
}

public class AbstractTaskService : IAbstractTaskService
{
    private readonly IAbstractTaskRepository repository;
    private readonly MessageSender sender;
    private readonly IDistributedCache cache;

    public AbstractTaskService(IAbstractTaskRepository repository, IDistributedCache cache)
    {
        this.repository = repository;
        sender = new MessageSender();
        this.cache = cache;

    }
    public async Task<Result<GetTaskResponse>> GetTask(GetTaskRequest request)
    {   
        AbstractTask? task;
        var taskBody = await cache.GetAsync($"{request.Id}");
        if (taskBody != null)
        {
            var taskString = Encoding.UTF8.GetString(taskBody).Split(",");
            var description = taskString[0];
            var ttl = taskString[1];
            var status = taskString[2];
            task = new AbstractTask
            {
                Description = description,
                TTLInMilliseconds = int.Parse(ttl),
                Status = status
            };
        }
        else
            task = await repository.GetTask(request.Id);
        
        if (task is null)
            return Result.NotFound<GetTaskResponse>($"Task with {request.Id} id not found");
        
        return Result.Ok(new GetTaskResponse
        {
            Description = task.Description,
            TTlInMilliseconds = task.TTLInMilliseconds,
            Status = task.Status
        });
    }

    public async Task<Result<AddTaskResponse>> AddTask(AddTaskRequest request)
    {
        var task = new AbstractTask
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            TTLInMilliseconds = request.TTLInMillisecond,
            Status = "Wait"
        };
        await sender.SendMessage(task);
        
        return Result.Ok(new AddTaskResponse { Id = task.Id} );
    }

    public async Task<Result<RetryTaskResponse>> RetryTask(RetryTaskRequest request)
    {
        var task = await repository.GetTask(request.Id);
        
        if (task is null)
            return Result.NotFound<RetryTaskResponse>($"Task with {request.Id} id not found");
        
        await sender.SendMessage(task);

        return Result.Ok(new RetryTaskResponse { Id = task.Id, Status = "Wait" });
    }
}