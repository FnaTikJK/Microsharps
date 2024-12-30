using Infrastructure.API.RightsChecker;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserRightsDto> UserRights { get; set; }  // Таблица прав пользователей
}