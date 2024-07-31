using System;
using FrostLib.Signals.impl;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Binding
{
    public class Binder<TConstraint>
    {
        internal Group Group { get; private set; } = new();

        internal EventType EventType { get; private set; }

        internal readonly Signal<Binder<TConstraint>> OnPropagatedSignal = new();

        private readonly Action<Group, EventType> _action;

        internal Binder(Action<Group, EventType> adder) => _action = adder;

        internal void Add(Type type) => Group.Add(type);

        public Binder<TConstraint> AsSequenceAsync()
        {
            Group.IsSequential = true;
            return this;
        }

        public Binder<TConstraint> AsSceneSwitchPersistent()
        {
            Group.CancelOnSceneSwitch = false;
            return this;
        }

        // Otherwise the hook will persist until scene is switched.
        // It will not cause errors, but having tons of hooks dispatched,
        // will slow down scene switch and cause memalloc spike.
        // This is optimization mostly for extensively called handlers.
        // Like any burst mode related handlers.
        // With this optimization, CancellationToken will not be canceled
        // after main body of Handler/Task has completed. 
        // Don't use this optimization if you pass the token to sub-tasks
        // that you don't await in main method.
        public Binder<TConstraint> DisposeSceneCancellationHookAfterHandling()
        {
            Group.DisposeSceneCancellationHookAfterHandling = true;
            return this;
        }

        public Binder<TConstraint> Once()
        {
            Group.UnbindAfterFirstExecution = true;
            return this;
        }

        public void To(EventType eventType)
        {
            EventType = eventType;
            Finish(eventType);
        }

        private void Finish(EventType eventType) => _action(Group, eventType);

        public Binder<TConstraint> Propagate()
        {
            var newBinder = new Binder<TConstraint>(_action)
            {
                EventType = EventType,
                Group = Group.Clone()
            };

            OnPropagatedSignal.Dispatch(newBinder);
            return newBinder;
        }
    }
}