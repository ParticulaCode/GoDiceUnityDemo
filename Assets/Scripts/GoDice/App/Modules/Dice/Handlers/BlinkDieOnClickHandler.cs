using GoDice.Shared.EventDispatching;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.EventDispatching.Injections;
using GoDice.Shared.Events.Dice;
using JetBrains.Annotations;
using UnityEngine;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class BlinkDieOnClickHandler : EventHandler
    {
        [Inject] private IEventDispatcher Dispatcher { get; set; }
        
        public BlinkDieOnClickHandler(DieClickedEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<DieClickedEvent>();
            Dispatcher.Raise(new DieBlinkEvent(ev.DieId, 8, Color.white, 0.1f, 0.1f, false));
        }
    }
}