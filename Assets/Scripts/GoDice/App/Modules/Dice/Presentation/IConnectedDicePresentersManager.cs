using System.Collections.Generic;

namespace GoDice.App.Modules.Dice.Presentation
{
    internal interface IConnectedDicePresentersManager
    {
        IEnumerable<IConnectedDicePresenter> Presenters { get; }

        void Add(IConnectedDicePresenter presenter);
        void Remove(IConnectedDicePresenter presenter);
    }
}