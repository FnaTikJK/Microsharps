using AbstractTaskWorker.Model;

namespace AbstractTaskWorker;

public class AbstractTaskRepository : IAbstractTaskRepository
{
    private readonly WorkerDbContext context;
    
    public AbstractTaskRepository(WorkerDbContext context)
    {
        this.context = context;
    }

    public async Task UpdateAsync(AbstractTaskW task)
    {
        context.AbstractTasks.Update(task);
        await context.SaveChangesAsync();
    }
    
}