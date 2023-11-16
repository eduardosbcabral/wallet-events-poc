using System.Text.Json;
using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;

namespace WalletEventsPoc.Events.Handlers;

// Record is needed because we compare handlers in dispatcher
// to avoid register two of the same one.
public record TransactionCreatedRabbitHandler : IEventHandler
{
    public RabbitMqProducer RabbitMqProducer { get; private set; }

    public TransactionCreatedRabbitHandler(RabbitMqProducer rabbitMqProducer)
    {
        RabbitMqProducer = rabbitMqProducer;
    }

    public async Task Handle(IEvent message)
    {
        await RabbitMqProducer.PublishAsync(message, "transactions-queue");
    }
}
