using System.Globalization;
using AbstractTaskService.Logic.Requests;
using AbstractTaskService.Logic.Response;
using AbstractTaskService.Models;
using AbstractTaskService.Models.Models;
using AbstractTaskService.Models.Request;
using AbstractTaskService.Models.Response;

namespace AbstractTaskService.API;

public static class ApiMapper
{
    public static AddTaskRequest Map(AddTaskRequestModel request)
        => new()
        {
            Description = request.Description,
            TTLInMillisecond = request.TTLInMillisecond
        };
    
    public static AddTaskResponseModel Map(AddTaskResponse response)
        => new()
        {
            Id = response.Id
        };

    public static GetTaskRequest Map(Guid id)
        => new()
        {
            Id = id
        };
    
    public static GetTaskResponseModel Map(GetTaskResponse response)
        => new()
        {
            Description = response.Description,
            TTlInMilliseconds = response.TTlInMilliseconds,
            Status = response.Status
        };

    public static RetryTaskRequest Map(RetryTaskRequestModel request)
        => new()
        {
            Id = request.Id
        };

    public static RetryTaskResponseModel Map(RetryTaskResponse response)
        => new()
        {
            Id = response.Id,
            Status = response.Status
        };


}