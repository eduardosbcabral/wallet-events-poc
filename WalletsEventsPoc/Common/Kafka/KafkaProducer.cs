using System.Text.Json;

namespace WalletEventsPoc.Common.Kafka;

public class KafkaProducer
{
    public Task PublishAsync(object msg, byte[] key, string topic)
    {
        Console.WriteLine("=======================");
        Console.WriteLine("KAFKA MESSAGE PUBLISHED");
        Console.WriteLine("message: " + JsonSerializer.Serialize(msg));
        Console.WriteLine("key: " + key);
        Console.WriteLine("topic: " + topic);
        Console.WriteLine("=======================");
        return Task.CompletedTask;
    }
}
