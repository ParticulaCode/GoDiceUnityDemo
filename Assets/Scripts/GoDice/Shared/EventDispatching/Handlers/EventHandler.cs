using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Handlers
{
    public abstract class EventHandler : EventHandlerBase, IHandler
    {
        public abstract void Handle();

        protected EventHandler(IEvent ev) : base(ev)
        {
        }
    }
}