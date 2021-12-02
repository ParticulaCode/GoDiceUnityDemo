using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieIgnoreDoubleTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieIgnoreDoubleTap;

        public DieIgnoreDoubleTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}