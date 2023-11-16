using System.Text.Json;
using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;

namespace WalletEventsPoc.Events.Handlers;

public record TransactionCreatedKafkaHandler : IEventHandler
{
    public KafkaProducer KafkaProducer { get; private set; }
    public RabbitMqProducer RabbitMqProducer { get; private set; }

    public TransactionCreatedKafkaHandler(KafkaProducer kafkaProducer, RabbitMqProducer rabbitMqProducer)
    {
        KafkaProducer = kafkaProducer;
        RabbitMqProducer = rabbitMqProducer;
    }

    public async Task Handle(IEvent message)
    {
        await KafkaProducer.PublishAsync(message, null, "transactions-topic");
        await RabbitMqProducer.PublishAsync(message, "transactions-queue");
        Console.Write("TransactionCreatedKafkaHandler: ", JsonSerializer.Serialize(message.Payload));
    }
}
