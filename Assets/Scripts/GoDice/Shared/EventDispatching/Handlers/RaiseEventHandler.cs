using System;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Injections;
using JetBrains.Annotations;

namespace GoDice.Shared.EventDispatching.Handlers
{
    [UsedImplicitly]
    public class RaiseEventHandler<T> : EventHandler where T : IEvent
    {
        [Inject] private IEventDispatcher Dispatcher { get; set; }

        public RaiseEventHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle() => Dispatcher.Raise(Activator.CreateInstance<T>());
    }
}