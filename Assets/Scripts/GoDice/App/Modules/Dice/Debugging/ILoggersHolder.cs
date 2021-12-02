using System;
using GoDice.App.Modules.Dice.Core;

namespace GoDice.App.Modules.Dice.Debugging
{
    internal interface ILoggersHolder
    {
        void Add(Die die);
        void Remove(Die die);
        DieLogger Get(Guid dieId);
    }
}