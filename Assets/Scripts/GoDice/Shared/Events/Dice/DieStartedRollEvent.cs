using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieStartedRollEvent : DieEvent
    {
        public override EventType Type => EventType.DieStartedRoll;

        public DieStartedRollEvent(Guid dieId) : base(dieId)
        {
        }
    }
}