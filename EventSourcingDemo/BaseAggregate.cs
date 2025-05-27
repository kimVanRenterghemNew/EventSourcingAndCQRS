namespace EventSourcingDemo;

public abstract class BaseAggregate<TEventInterface>
    where TEventInterface : Event
{
    private readonly List<TEventInterface> _events = [];
    private readonly Dictionary<Type, List<Action<Event>>> _routes = new ();

    protected IEnumerable<TEventInterface> Events => [.. _events];

    public async ValueTask PlayAllEvents(Func<TEventInterface, Task> writeEvent)
    {
        foreach (var @event in _events)
        {
            await writeEvent(@event);
        }
    }

    protected void RegisterHandler<TEvent>(Action<TEvent> handler)
        where TEvent : TEventInterface
    {
        if (!_routes.TryGetValue(typeof(TEvent), out var handlers))
        {
            handlers = [];
            _routes.Add(typeof(TEvent), handlers);
        }

        handlers.Add(x => handler((TEvent)x));
    }

    protected void PublishNewEvent(TEventInterface @event)
    {
        PlayEvent(@event);

        _events.Add(@event);
    }

    protected void PlayEvent(TEventInterface @event)
    {
        if (!_routes.TryGetValue(@event.GetType(), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers)
        {
            handler(@event);
        }
    }
}