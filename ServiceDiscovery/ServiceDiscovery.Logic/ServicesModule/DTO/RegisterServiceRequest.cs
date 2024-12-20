namespace ServiceDiscovery.Logic.ServicesModule.DTO;

public class RegisterServiceRequest
{
    public required string ServiceName { get; set; }
    public required string[] Hosts { get; set; }
}