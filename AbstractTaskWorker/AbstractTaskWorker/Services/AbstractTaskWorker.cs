using System.Text.Json;
using AbstractTaskWorker.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AbstractTaskWorker.Services;

public class AbstractTaskWorker : BackgroundService
{
    private readonly TaskExecutor executor;
    private readonly IServiceScopeFactory scopeFactory;
    private IConnection connection;
    private IChannel? channel;

    public AbstractTaskWorker(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
        executor = new TaskExecutor();
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
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
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
                var taskStatus = await executor.ExecuteTask(task);
                Console.WriteLine($"Id {task.Id} res: {taskStatus}");
                task.Status = taskStatus;
                using var scope = scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IAbstractTaskRepository>();
                await repository.UpdateAsync(task);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                try
                {
                    // Отправляем отрицательное подтверждение, чтобы сообщение можно было обработать снова
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
    
    private AbstractTaskW DeserializeToModelAsync(byte[] body)
    {
        using var memoryStream = new MemoryStream(body);
        var task = JsonSerializer.Deserialize<AbstractTaskW>(memoryStream);//TODO: проверка на валидность
        return task;
    }
}