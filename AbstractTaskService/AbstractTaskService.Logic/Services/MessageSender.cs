using System.Text.Json;
using AbstractTaskService.DAL.Entities;
using RabbitMQ.Client;

namespace AbstractTaskService.Logic.Services;

public class MessageSender
{
    private readonly ConnectionFactory factory;

    public MessageSender()
    {
        factory = new ConnectionFactory { HostName = "localhost" };
    }

    public async Task SendMessage(AbstractTask message)
    {
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        
        var messageBody = await SerializeToByte(message);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: messageBody);
    }
    
    private async Task<byte[]> SerializeToByte(AbstractTask obj)
    {
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, obj);
        return memoryStream.ToArray();
    }
}