using System.Text.Json;
using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;

namespace WalletEventsPoc.Events.Handlers;

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
