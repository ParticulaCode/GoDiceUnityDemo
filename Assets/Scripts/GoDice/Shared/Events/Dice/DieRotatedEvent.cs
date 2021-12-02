using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieRotatedEvent : DieEvent
    {
        public override EventType Type => EventType.DieRotated;

        public readonly int Value;

        public DieRotatedEvent(Guid id, int value) : base(id) => Value = value;
    }
}