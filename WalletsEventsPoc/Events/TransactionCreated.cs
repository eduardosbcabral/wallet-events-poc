using WalletEventsPoc.Common.Events;

namespace WalletEventsPoc.Events;

public record TransactionCreated : IEvent
{
    public string Name => nameof(TransactionCreated);
    public DateTime DateTime => DateTime.Now;
    public object Payload { get; set; }
}
