namespace Infrastructure.API.Configuration.ServiceDiscovery.Requests;

public class RegisterServiceResponseModel
{
    public required string[] Hosts { get; set; }
}