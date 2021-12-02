using System.Collections.Generic;

namespace GoDice.App.Modules.Dice.Presentation
{
    internal class PresentersManager : List<IConnectedDicePresenter>, IConnectedDicePresentersManager
    {
        public IEnumerable<IConnectedDicePresenter> Presenters => this;
        
        void IConnectedDicePresentersManager.Remove(IConnectedDicePresenter presenter) => base.Remove(presenter);
    }
}