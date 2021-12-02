using System;

namespace GoDice.App.Modules.Bluetooth.Devices.Data
{
    internal struct Info
    {
        public Guid Id;
        public string Address;
        public string Name;

        public override string ToString() => $"DeviceInfo: {Id}, {Address}, {Name}";
    }
}