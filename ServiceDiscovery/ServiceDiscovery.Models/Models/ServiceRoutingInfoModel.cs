namespace ServiceDiscovery.API.Modules.RoutingModule.Models;

public class ServiceRoutingInfoModel
{
    public required string ServiceName { get; set; }
    public required string[] Hosts { get; set; }
}