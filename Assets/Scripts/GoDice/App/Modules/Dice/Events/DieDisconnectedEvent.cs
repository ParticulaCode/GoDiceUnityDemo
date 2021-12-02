using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Events
{
    internal class DieDisconnectedEvent : DieEvent
    {
        public override EventType Type => EventType.DieDisconnected;

        public DieDisconnectedEvent(Die die) : base(die)
        {
        }
    }
}