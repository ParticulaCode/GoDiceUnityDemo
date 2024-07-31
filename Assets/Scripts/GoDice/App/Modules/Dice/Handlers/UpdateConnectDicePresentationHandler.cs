using System.Linq;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Presentation;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Handlers;
using GoDice.Shared.EventDispatching.Injections;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class UpdateConnectDicePresentationHandler : EventHandler
    {
        [Inject] private IDiceHolder DiceHolder { get; set; }
        [Inject] private IConnectedDicePresentersManager Presentator { get; set; }

        public UpdateConnectDicePresentationHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var dice = DiceHolder.GetConnectedDice().ToArray();
            foreach (var counter in Presentator.Presenters)
                counter.Present(dice);
        }
    }
}