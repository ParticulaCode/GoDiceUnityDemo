using System;

namespace GoDice.Shared.EventDispatching.Events
{
    public abstract class RoundTripEvent<T> : IEvent
    {
        public abstract EventType Type { get; }

        private readonly Action<T> _ok;
        private readonly Action<string> _err;

        protected RoundTripEvent(Action<T> ok, Action<string> err = null)
        {
            _ok = ok;
            _err = err;
        }

        public void Ok(T response) => _ok?.Invoke(response);

        public void Err(string message) => _err?.Invoke(message);
    }

    //TODO Refactor. It's used for the oboarding event only
    public abstract class RoundTripEvent : IEvent
    {
        public abstract EventType Type { get; }

        private readonly Action _callback;

        protected RoundTripEvent(Action cb) => _callback = cb;

        public void Respond() => _callback?.Invoke();
    }
}