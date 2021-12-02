using System;
using FrostLib.Signals.impl;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public interface IEventDispatcher
    {
        Signal<EventHandlerBase> OnHandlerCreatedSignal { get; }
        Signal<EventType> OnRaisingEventSignal { get; }
        
        Binder<EventHandlerBase> Bind<T>() where T : EventHandlerBase;
        
        Binder<EventHandlerBase> Unbind<T>() where T : EventHandlerBase;
        Binder<EventHandlerBase> Unbind(Type type);
        
        void Raise(IEvent ev);
    }
}    