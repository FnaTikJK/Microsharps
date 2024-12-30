namespace Infrastructure.API.RightsChecker;

public interface IRightsChecker
{
    Task<bool> HasAccessAsync(string userId, string resource, string action);
}