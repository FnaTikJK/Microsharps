using Infrastructure.API.Configuration.ServiceDiscovery.Requests;
using ServiceDiscovery.Logic.ServicesModule.DTO;
using ServiceDiscovery.Logic.ServicesModule.Models;
using ServiceDiscovery.Models.Models;
using ServiceDiscovery.Models.Requests;
using RegisterServiceRequest = ServiceDiscovery.Logic.ServicesModule.DTO.RegisterServiceRequest;
using RegisterServiceResponse = ServiceDiscovery.Logic.ServicesModule.DTO.RegisterServiceResponse;
using RemoveServiceRequest = ServiceDiscovery.Logic.ServicesModule.DTO.RemoveServiceRequest;

namespace ServiceDiscovery.API.Modules.RoutingModule;

public static class RoutingApiMapper
{
    public static RegisterServiceRequest Map(RegisterServiceRequestModel request)
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
    
    public static RemoveServiceRequest Map(RemoveServiceRequestModel request)
        => new()
        {
            ServiceName = request.ServiceName,
            Hosts = request.Hosts,
        };
}