using AbstractTaskService.Client;
using AbstractTaskService.Models;
using AbstractTaskService.Models.Request;
using ApiGateway.Logic.Requests;
using Infrastructure;

namespace ApiGateway.Logic;

public interface ITestService
{
    Task<Result<string>> Test();
    Task<Result<TestPostResponse>> TestPost(TestPostRequest request);
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

    public async Task<Result<TestPostResponse>> TestPost(TestPostRequest request)
    {
        var response = await taskServiceClient.TestPost(TestMapper.Map(request));
        if (!response.IsSuccess)
            return Result.ErrorFrom<TestPostResponseModel, TestPostResponse>(response);

        return Result.Ok(TestMapper.Map(response.Value));
    }
}