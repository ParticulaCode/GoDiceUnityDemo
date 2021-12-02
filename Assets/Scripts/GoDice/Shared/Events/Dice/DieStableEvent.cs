using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieStableEvent : DieValueEvent
    {
        public override EventType Type => EventType.DieStable;

        public DieStableEvent(Guid dieId, int result) : base(dieId, result) {}
    }
}