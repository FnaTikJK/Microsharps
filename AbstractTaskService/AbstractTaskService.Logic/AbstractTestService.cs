using AbstractTaskService.Logic.Models;
using AbstractTaskService.Logic.Requests;
using Infrastructure;

namespace AbstractTaskService.Logic;

public interface IAbstractTaskService
{
    Task<Result<string>> TestGet();
    Task<Result<TestPostResponse>> TestPost(TestPostRequest request);
}

public class AbstractTaskService : IAbstractTaskService
{
    private readonly List<AbstractTask> tasks;

    public AbstractTaskService()
    {
        tasks = new List<AbstractTask>();
    }

    public async Task<Result<string>> TestGet()
    {
        return Result.Ok("Success");
    }

    public async Task<Result<TestPostResponse>> TestPost(TestPostRequest request)
    {
        tasks.Add(new AbstractTask
        {
            Name = request.Name,
            Description = request.Description,
        });
        return Result.Ok(new TestPostResponse
        {
            Tasks = tasks.ToArray(),
        });
    }
}