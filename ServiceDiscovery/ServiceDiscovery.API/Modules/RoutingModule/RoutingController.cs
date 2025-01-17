using Infrastructure.API;
using Infrastructure.API.Configuration.ServiceDiscovery.Requests;
using Microsoft.AspNetCore.Mvc;
using ServiceDiscovery.Logic.ServicesModule;
using ServiceDiscovery.Models.Requests;

namespace ServiceDiscovery.API.Modules.RoutingModule;

[Route("api/[controller]")]
[ApiController]
public class RoutingController : ControllerBase
{
    private readonly IRoutingService routingService;

    public RoutingController(IRoutingService routingService)
    {
        this.routingService = routingService;
    }
    
    /// <summary>
    /// Регистрирует хост для сервиса
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RegisterServiceResponseModel>> RegisterService([FromBody] RegisterServiceRequestModel request)
    {
        var response = await routingService.RegisterHosts(RoutingApiMapper.Map(request));
        return response.ActionResult(RoutingApiMapper.Map);
    }

    /// <summary>
    /// Достаёт рандомный хост у сервиса
    /// </summary>
    [HttpGet("{serviceName}")]
    public ActionResult<GetServiceHostResponseModel> GetServiceHost([FromRoute] string serviceName)
    {
        var response = routingService.GetServiceHost(serviceName);
        return response.ActionResult(RoutingApiMapper.Map);
    }
    
    /// <summary>
    /// Достаёт все хосты у всех сервисов
    /// </summary>
    [HttpGet("all")]
    public ActionResult<GetAllServicesHostsResponseModel> GetAllServiceHosts()
    {
        var response = routingService.GetAllServicesWithHosts();
        return response.ActionResult(RoutingApiMapper.Map);
    }

    /// <summary>
    /// Чистит хост сервиса
    /// </summary>
    [HttpDelete]
    public ActionResult DeleteService([FromBody] RemoveServiceRequestModel request)
    {
        var response = routingService.RemoveHosts(RoutingApiMapper.Map(request));
        return response.ActionResult();
    }
}