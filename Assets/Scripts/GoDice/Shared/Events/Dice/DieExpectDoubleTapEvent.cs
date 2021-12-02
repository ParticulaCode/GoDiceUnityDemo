using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieExpectDoubleTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieExpectDoubleTap;

        public DieExpectDoubleTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}