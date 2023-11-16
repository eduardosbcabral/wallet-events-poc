using Moq;
using WalletEventsPoc.Common.Events;

namespace WalletsEventsPoc.Tests;

public class EventDispatcherTests
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IEvent _event1;
    private readonly IEvent _event2;
    private readonly IEventHandler _handler1;
    private readonly IEventHandler _handler2;
    private readonly IEventHandler _handler3;

    public EventDispatcherTests()
    {
        _eventDispatcher = new EventDispatcher();
        _event1 = new TestEvent()
        {
            Name = "test",
            Payload = "test"
        };
        _event2 = new TestEvent()
        {
            Name = "test2",
            Payload = "test2"
        };
        _handler1 = new TestEventHandler()
        {
            Id = 1
        };
        _handler2 = new TestEventHandler()
        {
            Id = 2
        };
        _handler3 = new TestEventHandler()
        {
            Id = 3
        };
    }

    [Fact]
    public void TestEventDispatcher_Register()
    {
        _eventDispatcher.Register(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);

        _eventDispatcher.Register(_event1.Name, _handler2);
        Assert.Equal(2, _eventDispatcher[_event1.Name].Count());

        Assert.Equal(_handler1, _eventDispatcher[_event1.Name].ElementAt(0));
        Assert.Equal(_handler2, _eventDispatcher[_event1.Name].ElementAt(1));
    }

    [Fact]
    public void TestEventDispatcher_Register_WithSameHandler()
    {
        _eventDispatcher.Register(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);

        Assert.Throws<EventException>(() =>
            _eventDispatcher.Register(_event1.Name, _handler1)
        );
        Assert.Single(_eventDispatcher[_event1.Name]);
    }

    [Fact]
    public void TestEventDispatcher_Has()
    {
        _eventDispatcher.Register(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);

        _eventDispatcher.Register(_event1.Name, _handler2);
        Assert.Equal(2, _eventDispatcher[_event1.Name].Count());

        Assert.True(_eventDispatcher.Has(_event1.Name, _handler1));
        Assert.True(_eventDispatcher.Has(_event1.Name, _handler2));
        Assert.False(_eventDispatcher.Has(_event1.Name, _handler3));
    }

    [Fact]
    public void TestEventDispatcher_Remove()
    {
        // Event 1
        _eventDispatcher.Register(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);

        _eventDispatcher.Register(_event1.Name, _handler2);
        Assert.Equal(2, _eventDispatcher[_event1.Name].Count());

        // Event 2
        _eventDispatcher.Register(_event2.Name, _handler3);
        Assert.Single(_eventDispatcher[_event2.Name]);

        _eventDispatcher.Remove(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);
        Assert.Equal(_handler2, _eventDispatcher[_event1.Name].ElementAt(0));

        _eventDispatcher.Remove(_event1.Name, _handler2);
        Assert.Empty(_eventDispatcher[_event1.Name]);

        _eventDispatcher.Remove(_event2.Name, _handler3);
        Assert.Empty(_eventDispatcher[_event2.Name]);
    }

    [Fact]
    public void TestEventDispatcher_Clear()
    {
        // Event 1
        _eventDispatcher.Register(_event1.Name, _handler1);
        Assert.Single(_eventDispatcher[_event1.Name]);

        _eventDispatcher.Register(_event1.Name, _handler2);
        Assert.Equal(2, _eventDispatcher[_event1.Name].Count());

        // Event 2
        _eventDispatcher.Register(_event2.Name, _handler3);
        Assert.Single(_eventDispatcher[_event2.Name]);

        _eventDispatcher.Clear();
        Assert.Empty(_eventDispatcher.Handlers);
    }

    [Fact]
    public async Task TestEventDispatcher_Dispatch()
    {
        var eh1 = new Mock<IEventHandler>();
        var eh2 = new Mock<IEventHandler>();

        _eventDispatcher.Register(_event1.Name, eh1.Object);
        _eventDispatcher.Register(_event1.Name, eh2.Object);

        await _eventDispatcher.Dispatch(_event1);

        eh1.Verify(x => x.Handle(It.Is<IEvent>(x => x == _event1)), Times.Once);
        eh2.Verify(x => x.Handle(It.Is<IEvent>(x => x == _event1)), Times.Once);
    }
}

record TestEvent : IEvent
{
    public string Name { get; set; }
    public DateTime DateTime => DateTime.Now;
    public object Payload { get; set; }
}

// Record is needed because we compare handlers in dispatcher
// to avoid register two of the same one.
record TestEventHandler : IEventHandler
{
    public int Id { get; set; }

    public Task Handle(IEvent ev)
        => Task.CompletedTask;
}