using AbstractTaskService.DAL.Context;
using AbstractTaskService.DAL.Entities;

namespace AbstractTaskService.DAL.Repositories;

public class AbstractTaskRepository : IAbstractTaskRepository
{
    private readonly AbstractTaskDbContext context;

    public AbstractTaskRepository(AbstractTaskDbContext context)
    {
        this.context = context;
    }

    public async Task<AbstractTask?> GetTask(Guid id)
        => await context.AbstractTasks.FindAsync(id); 
    
    public async Task CreateOrUpdate(AbstractTask task)
    {
        var abstractTask = await GetTask(task.Id);
        if (abstractTask is null)
        { 
            await context.AbstractTasks.AddAsync(task);
        }
        else
        {
            context.Entry(abstractTask).CurrentValues.SetValues(task);
        }

        await context.SaveChangesAsync();
    }
}