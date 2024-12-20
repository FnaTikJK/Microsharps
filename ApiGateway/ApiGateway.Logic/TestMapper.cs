using AbstractTaskService.Models.Models;
using AbstractTaskService.Models.Request;
using ApiGateway.Logic.Models;
using ApiGateway.Logic.Requests;

namespace ApiGateway.Logic;

public static class TestMapper
{
    public static TestPostRequestModel Map(TestPostRequest request)
        => new()
        {
            Name = request.Name,
            Description = request.Description,
        };
    
    public static TestPostResponse Map(TestPostResponseModel response)
        => new()
        {
            Tasks = response.Tasks.Select(Map).ToArray(),
        };

    private static AbstractTask Map(AbstractTaskModel task)
        => new()
        {
            Name = task.Name,
            Description = task.Description,
        };
}