namespace ServiceDiscovery.API.Modules.RoutingModule.DTO;

public class RemoveServiceRequestModel
{
    public required string ServiceName { get; set; }
    public required string[] Hosts { get; set; }
}