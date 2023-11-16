using System.Text.Json;
using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;

namespace WalletEventsPoc.Events.Handlers;

public record TransactionCreatedKafkaHandler : IEventHandler
{
    public KafkaProducer KafkaProducer { get; private set; }

    public TransactionCreatedKafkaHandler(KafkaProducer kafkaProducer)
    {
        KafkaProducer = kafkaProducer;
    }

    public async Task Handle(IEvent message)
    {
        await KafkaProducer.PublishAsync(message, null, "transactions-topic");
    }
}
