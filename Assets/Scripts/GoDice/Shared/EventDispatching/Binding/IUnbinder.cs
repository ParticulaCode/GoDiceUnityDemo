using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Binding
{
    public interface IUnbinder
    {
        void From(EventType eventType);
    }
}