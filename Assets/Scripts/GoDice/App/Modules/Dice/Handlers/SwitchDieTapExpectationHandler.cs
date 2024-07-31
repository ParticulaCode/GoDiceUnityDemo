using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Handlers;
using GoDice.Shared.EventDispatching.Injections;
using GoDice.Shared.Events.Dice;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class SwitchDieTapExpectationHandler : EventHandler
    {
        [Inject] private Holder DiceHolder { get; set; }

        public SwitchDieTapExpectationHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<DieEvent>();
            var die = DiceHolder.GetDie(ev.DieId);

            switch (ev.Type)
            {
                case EventType.DieExpectTap:
                    die.ExpectTap();
                    break;
                case EventType.DieIgnoreTap:
                    die.IgnoreTap();
                    break;
                case EventType.DieExpectDoubleTap:
                    die.ExpectDoubleTap();
                    break;
                case EventType.DieIgnoreDoubleTap:
                    die.IgnoreDoubleTap();
                    break;
            }
        }
    }
}