using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Binding
{
    public class GroupUnbinder : IUnbinder
    {
        private readonly Group _group;
        private readonly Action<Group, EventType> _action;
        private EventType _eventType;

        internal GroupUnbinder(Group group, Action<Group, EventType> action)
        {
            _group = group;
            _action = action;
        }

        void IUnbinder.From(EventType eventType)
        {
            _eventType = eventType;
            Finish();
        }

        private void Finish() => _action(_group, _eventType);
    }
}