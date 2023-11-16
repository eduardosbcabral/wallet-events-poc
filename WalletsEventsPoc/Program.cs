using Microsoft.AspNetCore.Mvc;
using WalletEventsPoc.Common.Events;
using WalletEventsPoc.Common.Kafka;
using WalletEventsPoc.Events;
using WalletEventsPoc.Events.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

var kafkaProducer = new KafkaProducer();
var rabbitProducer = new RabbitMqProducer();
var eventDispatcher = new EventDispatcher();
eventDispatcher.Register("TransactionCreated", new TransactionCreatedKafkaHandler(kafkaProducer, rabbitProducer));
eventDispatcher.Register("BalanceUpdated", new BalanceUpdatedKafkaHandler(kafkaProducer));

services.AddSingleton<IEventDispatcher>(eventDispatcher);
services.AddScoped<ITransactionService>(x => new TransactionService(eventDispatcher, new TransactionCreated(), new BalanceUpdated()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapPost("/transactions", ([FromServices] ITransactionService transactionService) =>
{
    var result = transactionService.Create();
    return result;
});

app.Run();

class TransactionService : ITransactionService
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IEvent _transactionCreated;
    private readonly IEvent _balanceUpdated;
    public TransactionService(
        IEventDispatcher eventDispatcher,
        IEvent transactionCreated,
        IEvent balanceUpdated)
    {
        _eventDispatcher = eventDispatcher;
        _transactionCreated = transactionCreated;
        _balanceUpdated = balanceUpdated;
    }

    public async Task<object> Create()
    {
        var result = new
        {
            Amount = new Random().NextDouble() * 100,
            From = "Eduardo",
            To = "Felipe"
        };
        _transactionCreated.Payload = result;
        await _eventDispatcher.Dispatch(_transactionCreated);

        _balanceUpdated.Payload = new
        {
            From = "Eduardo",
            To = "Felipe",
            BalanceFrom = 100,
            BalanceTo = 100 + result.Amount
        };
        await _eventDispatcher.Dispatch(_balanceUpdated);

        return result;
    }
}

interface ITransactionService
{
    Task<object> Create();
}