using AbstractTaskWorker.Model;

namespace AbstractTaskWorker;

public interface IAbstractTaskRepository
{   
    Task UpdateAsync(AbstractTaskW task);
}