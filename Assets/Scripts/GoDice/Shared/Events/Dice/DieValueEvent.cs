using System;

namespace GoDice.Shared.Events.Dice
{
    public abstract class DieValueEvent : DieEvent
    {
        public readonly int Value;

        protected DieValueEvent(Guid dieId, int value) : base(dieId) => Value = value;
    }
}