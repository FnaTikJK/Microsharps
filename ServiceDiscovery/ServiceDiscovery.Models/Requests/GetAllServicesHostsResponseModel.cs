using ServiceDiscovery.API.Modules.RoutingModule.Models;

namespace ServiceDiscovery.API.Modules.RoutingModule.DTO;

public class GetAllServicesHostsResponseModel
{
    public required ServiceRoutingInfoModel[] RoutingInfos { get; set; }
}