using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieIgnoreTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieIgnoreTap;

        public DieIgnoreTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}