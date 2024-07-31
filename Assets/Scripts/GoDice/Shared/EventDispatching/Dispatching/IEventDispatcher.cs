using System;
using FrostLib.Signals.impl;
using GoDice.Shared.EventDispatching.Binding;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Exceptions;
using GoDice.Shared.EventDispatching.Handlers;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public interface IEventDispatcher
    {
        Signal<EventHandlerBase, EventType> OnHandlerCreatedSignal { get; }
        Signal<EventType> OnRaisingEventSignal { get; }
        Signal<Exception, ExceptionType> OnCaughtExceptionSignal { get; }
        Signal<CancellationExceptionInfo> OnCancellationExceptionSignal { get; }

        Binder<EventHandlerBase> Bind();

        IUnbinder Unbind<T>() where T : EventHandlerBase;
        IUnbinder UnbindSequenceWith<T>() where T : EventHandlerBase;
        IUnbinder Unbind(Group type);

        void Raise(IEvent ev);
    }
}