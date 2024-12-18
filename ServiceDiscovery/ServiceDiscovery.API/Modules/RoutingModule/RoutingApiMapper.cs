using ServiceDiscovery.API.Logic.ServicesModule.DTO;
using ServiceDiscovery.API.Logic.ServicesModule.Models;
using ServiceDiscovery.API.Modules.RoutingModule.DTO;
using ServiceDiscovery.API.Modules.RoutingModule.Models;
using RegisterServiceRequest = ServiceDiscovery.API.Logic.ServicesModule.DTO.RegisterServiceRequest;
using RegisterServiceResponse = ServiceDiscovery.API.Logic.ServicesModule.DTO.RegisterServiceResponse;
using RemoveServiceRequest = ServiceDiscovery.API.Logic.ServicesModule.DTO.RemoveServiceRequest;

namespace ServiceDiscovery.API.Modules.RoutingModule;

public static class RoutingApiMapper
{
    public static RegisterServiceRequest Map(DTO.RegisterServiceRequestModel request)
        => new()
        {
            ServiceName = request.ServiceName,
            Hosts = request.Hosts,
        };
    
    public static RegisterServiceResponseModel Map(RegisterServiceResponse response)
        => new()
        {
            Hosts = response.Hosts,
        };

    public static GetServiceHostResponseModel Map(GetServiceHostResponse response)
        => new()
        {
            Host = response.Host,
        };

    public static GetAllServicesHostsResponseModel Map(ServiceRoutingInfo[] routingInfos)
        => new()
        {
            RoutingInfos = routingInfos.Select(Map).ToArray(),
        };

    private static ServiceRoutingInfoModel Map(ServiceRoutingInfo routingInfo)
        => new()
        {
            ServiceName = routingInfo.ServiceName,
            Hosts = routingInfo.Hosts,
        };
    
    public static RemoveServiceRequest Map(DTO.RemoveServiceRequestModel request)
        => new()
        {
            ServiceName = request.ServiceName,
            Hosts = request.Hosts,
        };
}