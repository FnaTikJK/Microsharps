using Infrastructure.API.RightsChecker.Repository;

namespace Infrastructure.API.RightsChecker
{
    public class RightsChecker(IRightsCheckerRepository rightsRepository) : IRightsChecker
    {
        public async Task<bool> HasAccessAsync(string userId, string resource, string action)
        {
            var userRights = await rightsRepository.GetUserRightsAsync(userId);
            
            return userRights.Any(right => right.Resource == resource && right.Action == action);
        }
    }
}