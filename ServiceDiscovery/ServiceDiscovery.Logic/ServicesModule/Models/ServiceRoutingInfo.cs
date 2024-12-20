namespace ServiceDiscovery.Logic.ServicesModule.Models;

public class ServiceRoutingInfo
{
    public required string ServiceName { get; set; }
    public required string[] Hosts { get; set; }
}