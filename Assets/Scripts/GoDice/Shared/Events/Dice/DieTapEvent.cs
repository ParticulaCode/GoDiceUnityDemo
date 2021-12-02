using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieTap;

        public DieTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}