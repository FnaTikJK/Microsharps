using System.Diagnostics;
using System.Text;
using System.Text.Json;
using AbstractTaskWorker.Model;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker;

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
        source.CancelAfter(task.TTLInMillisecond);
        try
        {
            Console.WriteLine("Запуск обработки");
            var delay = rnd.Next(500, 1500);
            var progressUpdateStep = delay / 20;
            stopwatch.Restart();
            while (stopwatch.ElapsedMilliseconds <= delay)
            {
                Console.WriteLine($"delay {delay}");
                await Task.Delay(progressUpdateStep, token);
                var progress = Math.Min(Math.Round((double)stopwatch.ElapsedMilliseconds / delay * 100),100);
                task.Status = $"In progress: {progress} % done";
                Console.WriteLine($"{task.Status}");
                await cache.SetStringAsync(task.Id.ToString(), task.Status);
                Console.WriteLine("Отправил в кеш");
            }
        }
        catch (OperationCanceledException)
        {
            task.Status = "Canceled: Timeout";
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}