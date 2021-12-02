using GoDice.App.Modules.Bluetooth.Devices;

namespace GoDice.App.Modules.Bluetooth.Scan
{
    internal interface IScanner
    {
        Sync<IDevice> GetNewDevicesSync();
        void SetMode(ScanMode mode);
    }
}