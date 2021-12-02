using System;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Scan;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Bluetooth.Events
{
    public class NewDevicesScanStartEvent : RoundTripEvent<Sync<IDevice>>, IScanStartEvent
    {
        public override EventType Type => EventType.NewDevicesScanStart;

        ScanMode IScanStartEvent.Mode => ScanMode.NewDevices;

        public NewDevicesScanStartEvent(Action<Sync<IDevice>> cb) : base(cb)
        {
        }
    }
}