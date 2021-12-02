using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieExpectTapEvent : DieEvent
    {
        public override EventType Type => EventType.DieExpectTap;

        public DieExpectTapEvent(Guid dieId) : base(dieId)
        {
        }
    }
}