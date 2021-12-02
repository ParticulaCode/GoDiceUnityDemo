using System;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Extensions;

namespace GoDice.App.Modules.Bluetooth.Devices
{
    internal class Holder : IDeviceHolder
    {
        private readonly List<IDevice> _devices = new List<IDevice>();

        void IDeviceHolder.Add(IDevice device) => _devices.AddDistinct(device);

        void IDeviceHolder.Remove(IDevice device)
        {
            if (!Exists(device))
                return;

            _devices.Remove(device);
        }

        private bool Exists(IDevice device) => _devices.Contains(device);

        public bool Exists(string address) => _devices.Any(d => d.Address == address);

        public IDevice Get(string address) => _devices.FirstOrDefault(d => d.Address == address);

        public IDevice Get(Guid id) => _devices.FirstOrDefault(d => d.Id == id);

        public IEnumerable<IDevice> GetAll() => _devices.ToArray();

        public bool AllDevicesConnected() => _devices.All(d => d.State == DeviceState.Connected);
    }
}