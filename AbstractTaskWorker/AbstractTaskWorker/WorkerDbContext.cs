using AbstractTaskWorker.Model;
using Microsoft.EntityFrameworkCore;

namespace AbstractTaskWorker;

public sealed class WorkerDbContext : DbContext
{
    public DbSet<AbstractTask> AbstractTasks { get; set; }

    public WorkerDbContext(DbContextOptions<WorkerDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}