namespace ServiceDiscovery.API.Logic.ServicesModule.DTO;

public class RemoveServiceRequest
{
    public required string ServiceName { get; set; }
    public required string Host { get; set; }
}