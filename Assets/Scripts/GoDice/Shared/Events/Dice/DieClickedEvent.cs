using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.Shared.Events.Dice
{
    public class DieClickedEvent : DieEvent
    {
        public override EventType Type => EventType.DieClicked;

        public DieClickedEvent(Guid dieId) : base(dieId)
        {
        }
    }
}