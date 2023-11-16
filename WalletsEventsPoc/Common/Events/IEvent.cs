namespace WalletEventsPoc.Common.Events;

public interface IEvent
{
    string Name { get; }
    DateTime DateTime { get; }
    object Payload { get; set; }
}
