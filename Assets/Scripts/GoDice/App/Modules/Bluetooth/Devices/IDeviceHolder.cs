using System;
using System.Collections.Generic;

namespace GoDice.App.Modules.Bluetooth.Devices
{
    public interface IDeviceHolder
    {
        IDevice Get(string address);
        IDevice Get(Guid id);
        IEnumerable<IDevice> GetAll();
        
        void Add(IDevice device);
        void Remove(IDevice device);
        bool Exists(string address);

        bool AllDevicesConnected();
    }
}