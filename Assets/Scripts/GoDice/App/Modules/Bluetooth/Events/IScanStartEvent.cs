using GoDice.App.Modules.Bluetooth.Scan;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Bluetooth.Events
{
    internal interface IScanStartEvent : IEvent
    {
        ScanMode Mode { get; }
    }
}