using System;
using GoDice.App.Modules.Dice.Messaging;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class TapModule : TapModuleBase
    {
        protected override Response Response { get; } = Response.Tap;
    }
}