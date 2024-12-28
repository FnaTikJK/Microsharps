using System.Text;
using System.Text.Json;
using AbstractTaskWorker.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker;

public class TaskExecutor
{
    private static readonly Random rnd = new();
    
    public async Task<string> ExecuteTask(AbstractTaskW taskW)
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        source.CancelAfter(taskW.TTLInMillisecond);
        
        string res;
        try
        {
            var delay = rnd.Next(500, 1500);
            Console.WriteLine($"delay {delay}");
            await Task.Delay(delay, token); //имитация работы
            res = "Done";
        }
        catch (OperationCanceledException)
        {
            res = "Canceled";
        }

        return res;
    }
}