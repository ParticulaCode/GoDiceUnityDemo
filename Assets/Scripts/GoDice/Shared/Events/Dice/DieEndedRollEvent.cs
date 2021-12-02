using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieEndedRollEvent : DieValueEvent
    {
        public override EventType Type => EventType.DieEndedRoll;

        public DieEndedRollEvent(Guid dieId, int result) : base(dieId, result) { }
    }
}