using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;

namespace WalletEventsPoc.Events.Handlers;

public record BalanceUpdatedKafkaHandler : IEventHandler
{
    public KafkaProducer Producer { get; private set; }

    public BalanceUpdatedKafkaHandler(KafkaProducer producer)
    {
        Producer = producer;
    }

    public async Task Handle(IEvent message)
    {
        await Producer.PublishAsync(message, null, "balances-queue");
        Console.WriteLine("UpdateBalanceKafkaHandler called");
    }
}
