using AbstractTaskService.Logic;
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
        var response = await abstractTaskService.Test();
        return response.ActionResult<string, string>();
    }
}