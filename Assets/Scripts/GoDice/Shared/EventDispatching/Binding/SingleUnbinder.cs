using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Binding
{
    public class SingleUnbinder<TConstraint> : IUnbinder
    {
        private readonly Action<Type, EventType> _action;
        private EventType _eventType;

        internal SingleUnbinder(Action<Type, EventType> action) => _action = action;

        void IUnbinder.From(EventType eventType)
        {
            _eventType = eventType;
            Finish();
        }

        private void Finish() => _action(typeof(TConstraint), _eventType);
    }
}