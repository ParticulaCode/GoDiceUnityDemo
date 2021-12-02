using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Operations;

namespace GoDice.App.Modules.Bluetooth.Devices
{
    public interface IDevice
    {
        Guid Id { get; }
        string Address { get; }
        string Name { get; }

        DeviceState State { get; }
        Signal<IDevice, DeviceState> OnStateChangedSignal { get; }
        Signal<byte[]> OnDataReceivedSignal { get; }

        void SendData(sbyte[] data, OperationType type);
    }
}