using Infrastructure.API.RightsChecker.Repository;

namespace Infrastructure.API.RightsChecker;

public class UserRightsService(IRightsCheckerRepository rightsRepository)
{
    public async Task AddRightToUser(string userId, string resource, string action)
    {
        var userRight = new UserRightsDto
        {
            UserId = userId,
            Resource = resource,
            Action = action
        };

        await rightsRepository.AddUserRightAsync(userRight);
    }
}