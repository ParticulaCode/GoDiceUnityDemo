using GoDice.App.Modules.Bluetooth.Devices;

namespace GoDice.App.Modules.Bluetooth
{
    internal partial class ReconnectProcessor
    {
        private class Record
        {
            public readonly IDevice Device;

            private bool _reconnectAttempted;

            public Record(IDevice device) => Device = device;

            public bool ShouldReconnect() => !_reconnectAttempted;

            public void Reset() => _reconnectAttempted = false;

            public void MarkReconnected() => _reconnectAttempted = true;
        }
    }
}