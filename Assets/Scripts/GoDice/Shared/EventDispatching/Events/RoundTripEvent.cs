using System;

namespace GoDice.Shared.EventDispatching.Events
{
    public abstract class RoundTripEvent<T> : IEvent
    {
        public abstract EventType Type { get; }

        private readonly Action<T> _callback;
        
        protected RoundTripEvent(Action<T> cb) => _callback = cb;

        public void Respond(T response) => _callback?.Invoke(response);
    }
    
    public abstract class RoundTripEvent : IEvent
    {
        public abstract EventType Type { get; }

        private readonly Action _callback;
        
        protected RoundTripEvent(Action cb) => _callback = cb;

        public void Respond() => _callback?.Invoke();
    }
}