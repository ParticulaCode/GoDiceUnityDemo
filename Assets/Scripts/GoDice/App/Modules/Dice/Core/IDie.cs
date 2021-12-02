using System;
using FrostLib.Containers.Rx;
using GoDice.Shared.Data;

namespace GoDice.App.Modules.Dice.Core
{
    public interface IDie
    {
        Guid Id { get; }
        string Address { get; }
        ReadonlyReactive<ShellType> Shell { get; }
        ReadonlyReactive<ColorType> Color { get; }
    }
}