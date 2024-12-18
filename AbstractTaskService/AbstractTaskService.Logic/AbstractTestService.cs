using Infrastructure;

namespace AbstractTaskService.Logic;

public interface IAbstractTaskService
{
    Task<Result<string>> Test();
}

public class AbstractTaskService : IAbstractTaskService
{
    public async Task<Result<string>> Test()
    {
        return Result.Ok("Success");
    }
}