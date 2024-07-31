using GoDice.App.Modules.Bluetooth.Events;
using GoDice.App.Modules.Bluetooth.Scan;
using GoDice.Shared.EventDispatching.Handlers;
using GoDice.Shared.EventDispatching.Injections;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Bluetooth
{
    [UsedImplicitly]
    internal class ScanStartHandler : EventHandler
    {
        [Inject] private IScanner Scanner { get; set; }

        public ScanStartHandler(IScanStartEvent ev) : base(ev)
        {
        }

        public override void Handle()
        {
            var ev = EventAs<NewDevicesScanStartEvent>();
            ev?.Ok(Scanner.GetNewDevicesSync());

            var mode = EventAs<IScanStartEvent>().Mode;
            Scanner.SetMode(mode);
        }
    }
}