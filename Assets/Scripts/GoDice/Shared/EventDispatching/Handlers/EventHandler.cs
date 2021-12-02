using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching
{
    public abstract class EventHandler : EventHandlerBase, IHandler
    {
        public abstract void Handle();

        protected EventHandler(IEvent ev) : base(ev)
        {
        }
    }
}