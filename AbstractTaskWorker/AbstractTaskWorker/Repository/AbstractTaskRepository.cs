using AbstractTaskWorker.Model;

namespace AbstractTaskWorker;

public class AbstractTaskRepository : IAbstractTaskRepository
{
    private readonly WorkerDbContext context;
    
    public AbstractTaskRepository(WorkerDbContext context)
    {
        this.context = context;
    }
    public async Task AddAsync(AbstractTask task)
    {
        await context.AbstractTasks.AddAsync(task);
        await context.SaveChangesAsync();
    }
    
}