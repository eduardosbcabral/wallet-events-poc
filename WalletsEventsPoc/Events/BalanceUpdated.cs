using WalletEventsPoc.Common.Events;

namespace WalletEventsPoc.Events;

record BalanceUpdated : IEvent
{
    public string Name => nameof(BalanceUpdated);
    public DateTime DateTime => DateTime.Now;
    public object Payload { get; set; }
}
