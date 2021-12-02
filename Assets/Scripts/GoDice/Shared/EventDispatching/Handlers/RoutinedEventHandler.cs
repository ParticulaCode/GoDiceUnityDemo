using System.Collections;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching
{
    public abstract class RoutinedEventHandler : EventHandlerBase, IRoutinedHandler
    {
        public abstract IEnumerator Handle();

        protected RoutinedEventHandler(IEvent ev) : base(ev)
        {
        }
    }
}