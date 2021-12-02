using FrostLib.Services;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Events;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Dice.Connectors
{
    internal class ConnectionBroadcaster : OnChangeDieListener<bool>
    {
        private static IEventDispatcher Dispatcher => ServiceLocator.Instance.Get<IEventDispatcher>();

        public ConnectionBroadcaster(Die die, Signal<bool> onChangeSignal) : base(die, onChangeSignal)
        {
        }

        protected override void OnChange(bool isOn)
        {
            var ev = isOn ? (IEvent) new DieConnectedEvent(Die) : new DieDisconnectedEvent(Die);
            Dispatcher.Raise(ev);
        }
    }
}