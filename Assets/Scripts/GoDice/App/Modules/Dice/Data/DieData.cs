using System;
using GoDice.Shared.Data;

namespace GoDice.App.Modules.Dice.Data
{
    public struct DieData
    {
        public Guid Id;
        public string Name;
        public ShellType Shell;
        public ColorType Color;
        public Guid DeviceId;

        public override string ToString() => $"DieData: ID {Id}, DeviceID {DeviceId}, {Name}, {Shell}, {Color})";
    }
}