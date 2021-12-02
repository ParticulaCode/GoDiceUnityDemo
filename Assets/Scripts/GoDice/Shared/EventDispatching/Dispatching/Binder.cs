using System;
using System.Collections.Generic;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public class Binder<TConstraint>
    {
        public IEnumerable<Type> Binds => _binds;
        public EventType EventType { get; private set; }

        private readonly List<Type> _binds = new List<Type>();
        private readonly Action<Type, EventType> _action;

        internal Binder(Action<Type, EventType> adder) => _action = adder;

        internal Binder<TConstraint> Bind<T>() where T : TConstraint => Bind(typeof(T));

        internal Binder<TConstraint> Bind(Type type)
        {
            _binds.Add(type);
            return this;
        }

        public Binder<TConstraint> Then<T>() where T : TConstraint => Bind<T>();

        public void To(EventType eventType) => Finish(eventType);

        public void From(EventType eventType) => Finish(eventType);

        private void Finish(EventType eventType)
        {
            EventType = eventType;

            foreach (var bind in _binds)
                _action(bind, eventType);
        }
    }
}