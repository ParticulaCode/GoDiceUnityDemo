using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching
{
    public abstract class EventHandlerBase
    {
        private readonly IEvent _event;

        protected EventHandlerBase(IEvent ev) => _event = ev;

        protected T EventAs<T>() where T : class => _event as T;

        protected bool EventIs<T>() => _event is T;
    }
}