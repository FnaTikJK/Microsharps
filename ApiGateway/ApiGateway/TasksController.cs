using AbstractTaskService.Models.Request;
using AbstractTaskService.Models.Response;
using ApiGateway.Logic.Tasks;
using Infrastructure.API;

namespace ApiGateway;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService abstractTaskService;

    public TasksController(ITaskService abstractTaskService)
    {
        this.abstractTaskService = abstractTaskService;
    }

    /// <summary>
    /// Получить задачу по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetTaskResponseModel>> GetTask([FromRoute] Guid id)
    {
        var response = await abstractTaskService.GetTask(id);
        return response.ActionResult();

    }
    
    /// <summary>
    /// Создать задачу и отправить на выполнение
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("AddTask")]
    public async Task<ActionResult<AddTaskResponseModel>> AddTask(AddTaskRequestModel request)
    {
        var response = await abstractTaskService.AddTask(request);
        return response.ActionResult();
    }
    
    /// <summary>
    /// Повторно отправить задачу
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("RetryTask")]
    public async Task<ActionResult<RetryTaskResponseModel>> RetryTask(RetryTaskRequestModel request)
    {
        var response = await abstractTaskService.RetryTask(request);
        return response.ActionResult();
    }
}