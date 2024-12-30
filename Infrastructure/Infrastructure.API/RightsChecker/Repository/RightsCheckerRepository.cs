
using Infrastructure.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.API.RightsChecker.Repository;

public class RightsCheckerRepository(ApplicationDbContext dbContext) : IRightsCheckerRepository
{
    public async Task<List<UserRightsDto>> GetUserRightsAsync(string userId)
    {
        return await dbContext.UserRights
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserRightsDto> GetUserRightAsync(string userId, string resource, string action)
    {
        return await dbContext.UserRights
            .FirstOrDefaultAsync(r => r.UserId == userId && r.Resource == resource && r.Action == action);
    }

    // Реализация метода для добавления прав
    public async Task AddUserRightAsync(UserRightsDto userRight)
    {
        if (userRight == null)
        {
            throw new ArgumentNullException(nameof(userRight), "User right cannot be null");
        }

        // Проверка на дублирование прав для того же пользователя и ресурса
        var existingRight = await dbContext.UserRights
            .FirstOrDefaultAsync(r => r.UserId == userRight.UserId && r.Resource == userRight.Resource && r.Action == userRight.Action);
        
        if (existingRight != null)
        {
            throw new InvalidOperationException("User already has this right.");
        }

        // Добавление нового права в базу
        await dbContext.UserRights.AddAsync(userRight);
        await dbContext.SaveChangesAsync();
    }
}
