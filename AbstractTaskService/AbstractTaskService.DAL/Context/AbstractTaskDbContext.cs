using AbstractTaskService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbstractTaskService.DAL.Context;

public sealed class AbstractTaskDbContext : DbContext
{
    public DbSet<AbstractTask> AbstractTasks { get; set; }

    public AbstractTaskDbContext(DbContextOptions<AbstractTaskDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
}