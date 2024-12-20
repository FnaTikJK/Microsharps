using System.Globalization;
using AbstractTaskService.Logic.Models;
using AbstractTaskService.Logic.Requests;
using AbstractTaskService.Models;
using AbstractTaskService.Models.Models;
using AbstractTaskService.Models.Request;

namespace AbstractTaskService.API;

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