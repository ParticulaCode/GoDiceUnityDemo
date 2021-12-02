using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Events
{
    internal class DieConnectedEvent : DieEvent
    {
        public override EventType Type => EventType.DieConnected;

        public DieConnectedEvent(Die die) : base(die)
        {
        }
    }
}