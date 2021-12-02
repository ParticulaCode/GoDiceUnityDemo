using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching;
using GoDice.Shared.EventDispatching.Injections;
using GoDice.Shared.Events.Dice;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class BlinkDieHandler : EventHandler
    {
        [Inject] private Holder DiceHolder { get; set; }

        public BlinkDieHandler(DieBlinkEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<DieBlinkEvent>();
            var die = DiceHolder.GetDie(ev.DieId);
            die.Led.Blink(ev.BlinksAmount, ev.Color, ev.OnDuration, ev.OffDuration, ev.IsMixed);
        }
    }
}