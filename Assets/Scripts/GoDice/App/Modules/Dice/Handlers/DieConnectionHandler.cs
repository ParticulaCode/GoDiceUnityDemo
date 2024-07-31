using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Events;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Handlers;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class DieConnectionHandler : EventHandler
    {
        public DieConnectionHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<DieConnectedEvent>();
            var isConnected = ev != null;
            if (isConnected)
                OnConnection(ev.Die);
        }

        private void OnConnection(Die die) => die.SendInitializationMessage();
    }
}