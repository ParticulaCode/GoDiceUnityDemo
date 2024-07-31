using System;
using GoDice.Shared.EventDispatching.Binding;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.EventDispatching.Exceptions
{
    public readonly struct CancellationExceptionInfo
    {
        public readonly OperationCanceledException Exception;
        public readonly Group Group;
        public readonly EventType EventType;

        public CancellationExceptionInfo(OperationCanceledException exception, Group group, EventType eventType)
        {
            Exception = exception;
            Group = group;
            EventType = eventType;
        }
    }
}