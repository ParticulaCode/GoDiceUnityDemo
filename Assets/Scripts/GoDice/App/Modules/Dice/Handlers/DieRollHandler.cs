using System;
using GoDice.App.Modules.Dice.Presentation;
using GoDice.App.Modules.Dice.Views;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.Events.Dice;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    public class DieRollHandler : Shared.EventDispatching.EventHandler
    {
        private readonly EventType _evType;

        public DieRollHandler(IEvent ev) : base(ev) => _evType = ev.Type;

        public override void Handle()
        {
            switch (_evType)
            {
                case EventType.DieStartedRoll:
                    HandleDieRoll();
                    break;
                case EventType.DieEndedRoll:
                case EventType.DieStable:
                    NotifyDieStopped();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleDieRoll()
        {
            var dieId = EventAs<DieEvent>().DieId;
            GetView(dieId)?.StartRoll();
        }

        private void NotifyDieStopped()
        {
            var ev = EventAs<DieValueEvent>();
            GetView(ev.DieId)?.EndRoll(ev.Value);
        }

        private static Die2dView GetView(Guid dieId) =>
            UnityEngine.Object.FindObjectOfType<ViewPresenter>().GetView(dieId);
    }
}