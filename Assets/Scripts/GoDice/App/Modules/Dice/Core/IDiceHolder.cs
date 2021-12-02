using System;
using System.Collections.Generic;

namespace GoDice.App.Modules.Dice.Core
{
    public interface IDiceHolder
    {
        IDie GetDie(Guid id);
        IEnumerable<IDie> GetConnectedDice();
    }
}