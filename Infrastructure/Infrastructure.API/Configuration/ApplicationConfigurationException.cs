namespace Infrastructure.API.Configuration;

public class ApplicationConfigurationException : Exception
{
    public ApplicationConfigurationException()
    {
    }
    
    public ApplicationConfigurationException(string error) : base(error)
    {
    }
}