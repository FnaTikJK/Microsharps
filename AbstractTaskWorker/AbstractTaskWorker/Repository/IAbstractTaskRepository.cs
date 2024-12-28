using AbstractTaskWorker.Model;

namespace AbstractTaskWorker;

public interface IAbstractTaskRepository
{   
    Task AddAsync(AbstractTask task);
}