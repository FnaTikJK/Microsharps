using ServiceDiscovery.Models.Models;

namespace ServiceDiscovery.Models.Requests;

public class GetAllServicesHostsResponseModel
{
    public required ServiceRoutingInfoModel[] RoutingInfos { get; set; }
}