using ApiGateway.Logic.Models;
using ApiGateway.Logic.Requests;
using ApiGateway.Models.Models;
using ApiGateway.Models.Requests;

namespace ApiGateway;

public static class TestApiMapper
{
    public static TestPostRequest Map(TestPostRequestModel request)
        => new()
        {
            Name = request.Name,
            Description = request.Description,
        };

    public static TestPostResponseModel Map(TestPostResponse response)
        => new()
        {
            Tasks = response.Tasks.Select(Map).ToArray(),
        };

    private static AbstractTaskModel Map(AbstractTask task)
        => new()
        {
            Name = task.Name,
            Description = task.Description,
        };
}