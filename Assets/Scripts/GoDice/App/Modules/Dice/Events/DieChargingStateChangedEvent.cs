using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Events
{
    internal class DieChargingStateChangedEvent : DieEvent
    {
        public override EventType Type => EventType.DieChargingStateChanged;
        
        public readonly bool IsOn;

        public DieChargingStateChangedEvent(Die die, bool isOn) : base(die) => IsOn = isOn;
    }
}