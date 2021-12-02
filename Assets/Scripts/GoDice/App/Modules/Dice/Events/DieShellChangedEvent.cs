using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Events
{
    internal class DieShellChangedEvent : DieEvent
    {
        public override EventType Type => EventType.DieShellChanged;

        public DieShellChangedEvent(Die die) : base(die)
        {
        }
    }
}