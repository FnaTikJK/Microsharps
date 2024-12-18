using ApiGateway.Logic;
using Infrastructure.API;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestService testService;

    public TestController(ITestService testService)
    {
        this.testService = testService;
    }

    [HttpGet("Test")]
    public async Task<ActionResult<string>> Test()
    {
        var response = await testService.Test();
        return response.ActionResult();
    }
}