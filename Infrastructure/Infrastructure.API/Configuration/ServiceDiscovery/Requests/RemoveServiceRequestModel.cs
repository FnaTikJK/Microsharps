namespace Infrastructure.API.Configuration.ServiceDiscovery.Requests;

public class RemoveServiceRequestModel
{
    public required string ServiceName { get; set; }
    public required string[] Hosts { get; set; }
}