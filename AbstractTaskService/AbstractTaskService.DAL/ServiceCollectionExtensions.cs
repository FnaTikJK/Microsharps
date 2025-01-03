using AbstractTaskService.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbstractTaskService.DAL;

public static class ServiceCollectionExtensions
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {   
        services.AddDbContext<AbstractTaskDbContext>(options
            => options.UseNpgsql(connectionString));
    }
    
}