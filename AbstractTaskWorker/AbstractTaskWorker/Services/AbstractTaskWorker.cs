using System.Text.Json;
using AbstractTaskService.DAL.Entities;
using AbstractTaskService.DAL.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker.Services;

public class AbstractTaskWorker : BackgroundService
{
    private readonly TaskExecutor executor;
    private IConnection connection;
    private IChannel? channel;
    private IAbstractTaskRepository repository;
    
    public AbstractTaskWorker(IDistributedCache cache, IServiceScopeFactory scopeFactory )
    {
        executor = new TaskExecutor(cache);
        var scope = scopeFactory.CreateScope();
        repository = scope.ServiceProvider.GetRequiredService<IAbstractTaskRepository>();
    }

    private async Task InitAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        connection = await factory.CreateConnectionAsync();
        channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(
            queue: "hello", 
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {   
        await InitAsync();
        var consumer = new AsyncEventingBasicConsumer(channel);
        await ReceiveMessage(consumer);
        await channel.BasicConsumeAsync(queue: "hello", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }
    private Task ReceiveMessage(AsyncEventingBasicConsumer consumer)
    {   
        consumer.ReceivedAsync += async (model, ea) =>
        {
            Console.WriteLine("получил сообщение");
            try
            {   
                var body = ea.Body.ToArray();
                var task = DeserializeToModelAsync(body);
                await executor.ExecuteTask(task);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
                await repository.CreateOrUpdate(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
                try
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
                catch (Exception nackEx)
                {
                    Console.WriteLine($"Ошибка отправки Nack: {nackEx.Message}");
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