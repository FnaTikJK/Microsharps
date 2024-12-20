using AbstractTaskService.Logic;
using AbstractTaskService.Models;
using AbstractTaskService.Models.Request;
using Infrastructure.API;
using Microsoft.AspNetCore.Mvc;

namespace AbstractTaskService.API;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IAbstractTaskService abstractTaskService;

    public TestController(IAbstractTaskService abstractTaskService)
    {
        this.abstractTaskService = abstractTaskService;
    }

    [HttpGet("Test")]
    public async Task<ActionResult<string>> Test()
    {
        var response = await abstractTaskService.TestGet();
        return response.ActionResult<string, string>();
    }

    [HttpPost("TestPost")]
    public async Task<ActionResult<TestPostResponseModel>> TestPost(TestPostRequestModel request)
    {
        var response = await abstractTaskService.TestPost(TestApiMapper.Map(request));
        return response.ActionResult(TestApiMapper.Map);
    }
}