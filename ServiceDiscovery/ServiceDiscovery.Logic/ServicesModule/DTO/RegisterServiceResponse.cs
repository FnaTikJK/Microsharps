namespace ServiceDiscovery.API.Logic.ServicesModule.DTO;

public class RegisterServiceResponse // TODO: all responses should be  structs
{
    public required string[] Hosts { get; set; }
}