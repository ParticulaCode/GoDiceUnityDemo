using System;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Coroutines;
using FrostLib.Extensions;
using FrostLib.Signals.impl;
using GoDice.Shared.EventDispatching.Events;
using UnityEngine;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public class EventDispatcher : IEventDispatcher
    {
        public Signal<EventHandlerBase> OnHandlerCreatedSignal { get; } =
            new Signal<EventHandlerBase>();
        public Signal<Events.EventType> OnRaisingEventSignal { get; } = new Signal<Events.EventType>();

        private readonly IDictionary<Events.EventType, List<Type>> _handlers =
            Enum<Events.EventType>.GetEnumValues()
                .ToDictionary(evType => evType, evType => new List<Type>());

        private readonly IRoutineRunner _routiner;

        public EventDispatcher(IRoutineRunner routiner) => _routiner = routiner;

        Binder<EventHandlerBase> IEventDispatcher.Bind<T>() =>
            new Binder<EventHandlerBase>(AddHandler).Bind<T>();

        private void AddHandler(Type type, Events.EventType eventType) =>
            _handlers[eventType].AddDistinct(type);

        public Binder<EventHandlerBase> Unbind<T>() where T : EventHandlerBase => Unbind(typeof(T));

        public Binder<EventHandlerBase> Unbind(Type type) =>
            new Binder<EventHandlerBase>(RemoveHandler).Bind(type);

        private void RemoveHandler(Type type, Events.EventType eventType) =>
            _handlers[eventType].Remove(type);

        void IEventDispatcher.Raise(IEvent ev)
        {
            var types = _handlers[ev.Type].ToArray();
            if (types.Length == 0)
            {
                if(Debug.isDebugBuild)
                    Debug.LogWarning($"Handler is missing for the event: {ev.Type}.");
                return;
            }

            OnRaisingEventSignal.Dispatch(ev.Type);
            
            foreach (var handlerType in types)
                ProcessEvent(ev, handlerType);
        }

        private void ProcessEvent(IEvent ev, Type handlerType)
        {
            var handlerBase = CreateHandler(ev, handlerType);
            switch (handlerBase)
            {
                case IHandler handler:
                {
                    handler.Handle();
                    break;
                }
                case IRoutinedHandler routinedHandler:
                {
                    //Should it be added in queue to the Routined.Runner instead?
                    _routiner.StartRoutine(routinedHandler.Handle());
                    break;
                }
            }
        }

        private EventHandlerBase CreateHandler(IEvent ev, Type handlerType)
        {
            var handler = (EventHandlerBase) Activator.CreateInstance(handlerType, ev);
            var injector = new ServicesInjector<EventHandlerBase>();
            injector.Inject(handler);

            OnHandlerCreatedSignal.Dispatch(handler);

            return handler;
        }
    }
}