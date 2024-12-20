using System.ComponentModel;

namespace Infrastructure.API.Configuration.ServiceDiscovery.Requests;

public class RegisterServiceRequestModel
{
    public required string ServiceName { get; set; }
    //[RegularExpression(Regexps.IPv4)] TODO
    [DefaultValue("127.0.0.1:8888")]
    public required string[] Hosts { get; set; }
}