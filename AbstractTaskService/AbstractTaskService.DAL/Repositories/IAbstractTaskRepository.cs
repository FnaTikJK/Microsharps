using AbstractTaskService.DAL.Entities;

namespace AbstractTaskService.DAL.Repositories;

public interface IAbstractTaskRepository
{
    Task<AbstractTask?> GetTask(Guid id);
    Task CreateOrUpdate(AbstractTask task);
}