using System;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Bluetooth.Events
{
    public class ConnectToDeviceRequestEvent : RoundTripEvent<bool>
    {
        public override EventType Type => EventType.ConnectToDeviceRequest;

        public readonly IDevice Device;

        public ConnectToDeviceRequestEvent(IDevice device, Action<bool> cb = null) : base(cb) =>
            Device = device;
    }
}