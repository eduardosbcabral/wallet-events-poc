namespace WalletEventsPoc.Common.Events;

public interface IEventHandler
{
    Task Handle(IEvent message);
}
