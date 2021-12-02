using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Bluetooth.Events
{
    public class NewDeviceConnectedEvent : IEvent
    {
        public EventType Type => EventType.BluetoothDeviceConnected;

        public readonly IDevice Device;

        public NewDeviceConnectedEvent(IDevice device) => Device = device;
    }
}