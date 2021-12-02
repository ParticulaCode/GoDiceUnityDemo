using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.Shared.EventDispatching;
using GoDice.Shared.EventDispatching.Injections;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Bluetooth
{
    [UsedImplicitly]
    internal class ConnectToDeviceRequestHandler : EventHandler
    {
        [Inject] private IDeviceConnector Connector { get; set; }

        public ConnectToDeviceRequestHandler(ConnectToDeviceRequestEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<ConnectToDeviceRequestEvent>();
            Connector.Connect(ev.Device, ev.Respond);
        }
    }
}