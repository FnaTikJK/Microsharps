using System.Diagnostics;
using AbstractTaskService.DAL.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace AbstractTaskWorker.Services;

public class TaskExecutor
{
    private static readonly Random rnd = new();
    private readonly Stopwatch stopwatch = new();
    private readonly IDistributedCache cache;
    public TaskExecutor(IDistributedCache cache)
    {
        this.cache = cache;
    }
    
    public async Task ExecuteTask(AbstractTask task)
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        source.CancelAfter(task.TTLInMilliseconds);
        try
        {
            Console.WriteLine("Запуск обработки");
            var delay = rnd.Next(60000, 180000);
            var progressUpdateStep = delay / 20;
            stopwatch.Restart();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
            while (stopwatch.ElapsedMilliseconds <= delay)
            {
                Console.WriteLine($"delay {delay}");
                await Task.Delay(progressUpdateStep, token);
                var progress = Math.Min(Math.Round((double)stopwatch.ElapsedMilliseconds / delay * 100),100);
                task.Status = $"In progress: {progress} % done";
                Console.WriteLine($"{task.Status}");
                await cache.SetStringAsync(task.Id.ToString(), $"{task.Description},{task.TTLInMilliseconds},{task.Status}");
                Console.WriteLine("Отправил в кеш");
            }
        }
        catch (OperationCanceledException)
        {
            task.Status = "Canceled: Timeout";
        }
        finally
        {
            await cache.RemoveAsync(task.Id.ToString());
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
        }
    }
}