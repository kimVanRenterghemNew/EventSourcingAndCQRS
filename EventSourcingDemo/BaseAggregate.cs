namespace EventSourcingDemo
{
    public abstract class BaseAggregate<TEvent>
        where TEvent : Event
    {
        private readonly List<TEvent> _events = new List<TEvent>();
        private readonly Dictionary<Type, List<Action<Event>>> _routes = new Dictionary<Type, List<Action<Event>>>();

        protected List<TEvent> Events => _events.ToList();

        public async Task PlayAllEvents(Func<TEvent, Task> writeEvent)
        {
            foreach (var @event in _events)
            {
                await writeEvent(@event);
            }
        }

        protected void RegisterHandler<TEvent>(Action<TEvent> handler)
            where TEvent : Event
        {
            if (!_routes.TryGetValue(typeof(TEvent), out var handlers))
            {
                handlers = new List<Action<Event>>();
                _routes.Add(typeof(TEvent), handlers);
            }

            handlers.Add(x => handler((TEvent)x));
        }

        protected void PublishNewEvent(TEvent @event)
        {
            PlayEvent(@event);

            _events.Add(@event);
        }

        protected void PlayEvent(TEvent @event)
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
}
