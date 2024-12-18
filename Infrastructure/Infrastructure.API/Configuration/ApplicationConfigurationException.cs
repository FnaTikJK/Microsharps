namespace Infrastructure.API.Configuration.Application;

public class ApplicationConfigurationException : Exception
{
    public ApplicationConfigurationException()
    {
    }
    
    public ApplicationConfigurationException(string error) : base(error)
    {
    }
}