using System.Text.Json;

namespace WalletEventsPoc.Common.Kafka;

public class RabbitMqProducer
{
    public Task PublishAsync(object msg, string queueName)
    {
        Console.WriteLine("=======================");
        Console.WriteLine("RABBITMQ MESSAGE PUBLISHED");
        Console.WriteLine("message: " + JsonSerializer.Serialize(msg));
        Console.WriteLine("queueName: " + queueName);
        Console.WriteLine("=======================");
        return Task.CompletedTask;
    }
}
