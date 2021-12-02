using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieDoubleTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieDoubleTap;

        public DieDoubleTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}