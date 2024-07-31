using FrostLib.Containers;
using GoDice.Shared.EventDispatching.Binding;
using GoDice.Shared.EventDispatching.Handlers;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public class HandlersBlock : DisposableGroup
    {
        private readonly IEventDispatcher _dispatcher;

        public HandlersBlock(IEventDispatcher dispatcher) => _dispatcher = dispatcher;

        public Binder<EventHandlerBase> Bind()
        {
            var binder = _dispatcher.Bind();
            Subscribe(binder);
            return binder;
        }

        private void Subscribe(Binder<EventHandlerBase> binder)
        {
            binder.OnPropagatedSignal.AddListener(Subscribe);

            Add(() =>
            {
                binder.OnPropagatedSignal.RemoveListener(Subscribe);
                _dispatcher.Unbind(binder.Group).From(binder.EventType);
            });
        }
    }
}