using Infrastructure;

namespace Infrastructure.API.RightsChecker.Repository;

public interface IRightsCheckerRepository
{
    Task<List<UserRightsDto>> GetUserRightsAsync(string userId);
    Task<UserRightsDto> GetUserRightAsync(string userId, string resource, string action);
    Task AddUserRightAsync(UserRightsDto userRight);
}