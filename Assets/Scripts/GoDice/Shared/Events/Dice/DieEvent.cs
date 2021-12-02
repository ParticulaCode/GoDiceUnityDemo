using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public abstract class DieEvent : IEvent
    {
        public abstract EventType Type { get; }
        public readonly Guid DieId;

        protected DieEvent(Guid dieId) => DieId = dieId;
    }
}