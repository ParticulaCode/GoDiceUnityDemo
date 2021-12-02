using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Events
{
    internal abstract class DieEvent : IEvent
    {
        public abstract EventType Type { get; }

        public readonly Die Die;

        protected DieEvent(Die die) => Die = die;
    }
}