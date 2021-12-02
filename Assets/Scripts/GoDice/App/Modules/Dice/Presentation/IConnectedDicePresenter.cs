using System.Collections.Generic;
using GoDice.App.Modules.Dice.Core;

namespace GoDice.App.Modules.Dice.Presentation
{
    public interface IConnectedDicePresenter
    {
        void Present(IEnumerable<IDie> dice);
    }
}