namespace WalletEventsPoc.Common.Events;

public class EventDispatcher : IEventDispatcher
{
    public IList<KeyValuePair<string, IEventHandler>> Handlers { get; private set; }

    public EventDispatcher()
    {
        Handlers = new List<KeyValuePair<string, IEventHandler>>();
    }

    public IEnumerable<IEventHandler> this[string key]
    {
        get => Handlers.Where(x => x.Key == key).Select(x => x.Value);
    }

    public void Register(string eventName, IEventHandler handler)
    {
        var existentHandler = this[eventName].Any(x => x.Equals(handler));
        if (existentHandler)
        {
            throw new EventException("Event handler already registered.");
        }

        Handlers.Add(new(eventName, handler));
    }

    public bool Has(string eventName, IEventHandler handler)
        => this[eventName].Any(x => x.Equals(handler));

    public void Remove(string eventName, IEventHandler handler)
    {
        var existentHandler = Handlers
            .Where(x => x.Key == eventName)
            .FirstOrDefault(x => x.Value.Equals(handler));
        if (existentHandler.Value is not null)
        {
            Handlers.Remove(existentHandler);
        }
    }

    public void Clear()
    {
        Handlers = new List<KeyValuePair<string, IEventHandler>>();
    }

    public async Task Dispatch(IEvent ev)
    {
        if (this[ev.Name].Any())
        {
            foreach (var handler in this[ev.Name])
            {
                await Task.Run(async () =>
                {
                    await handler.Handle(ev);
                });
            }
        }
    }
}

public interface IEventDispatcher
{
    IList<KeyValuePair<string, IEventHandler>> Handlers { get; }

    void Register(string eventName, IEventHandler handler);
    bool Has(string eventName, IEventHandler handler);
    void Remove(string eventName, IEventHandler handler);
    void Clear();
    Task Dispatch(IEvent ev);

    IEnumerable<IEventHandler> this[string key] { get; }
}