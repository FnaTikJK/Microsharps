using System.Text.Json;
using AbstractTaskWorker.Model;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker.Services;

public class AbstractTaskWorker : BackgroundService
{
    private readonly TaskExecutor executor;
    private readonly IServiceScopeFactory scopeFactory;
    private IConnection connection;
    private IChannel? channel;
    private IDistributedCache cache;
    
    public AbstractTaskWorker(IDistributedCache cache, IServiceScopeFactory scopeFactory )
    {
        this.cache = cache;
        this.scopeFactory = scopeFactory;
        executor = new TaskExecutor(cache);
    }

    private async Task InitAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        connection = await factory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (channel is null)
            await InitAsync();
        var consumer = new AsyncEventingBasicConsumer(channel);
        while (!stoppingToken.IsCancellationRequested)
        {

            await ReceiveMessage(consumer);
            await channel.BasicConsumeAsync(queue: "hello", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        }
    }
    
    private Task ReceiveMessage(AsyncEventingBasicConsumer consumer)
    {   
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {   
                var body = ea.Body.ToArray();
                var task = DeserializeToModelAsync(body);
                await executor.ExecuteTask(task);
                using var scope = scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IAbstractTaskRepository>();
                await repository.AddAsync(task);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                try
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
                catch (Exception nackEx)
                {
                    Console.WriteLine($"Error sending Nack: {nackEx.Message}");
                }
            }
        };
        return Task.CompletedTask;
    }
    
    private AbstractTask DeserializeToModelAsync(byte[] body)
    {
        using var memoryStream = new MemoryStream(body);
        var task = JsonSerializer.Deserialize<AbstractTask>(memoryStream);//TODO: проверка на валидность
        return task;
    }
}