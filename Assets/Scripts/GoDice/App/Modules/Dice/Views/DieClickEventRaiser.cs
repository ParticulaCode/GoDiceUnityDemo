using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.Components;
using GoDice.Shared.Events.Dice;

namespace GoDice.App.Modules.Dice.Views
{
    internal class DieClickEventRaiser : EventOnClickBase
    {
        private IDie _die;

        public void SetDie(IDie die) => _die = die;

        protected override void TryRaiseEvent() => Raise(new DieClickedEvent(_die.Id));
    }
}