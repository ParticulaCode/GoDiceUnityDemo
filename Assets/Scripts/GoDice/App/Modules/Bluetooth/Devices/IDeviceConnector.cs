namespace GoDice.App.Modules.Bluetooth.Devices
{
    internal interface IDeviceConnector
    {
        void Connect(IDevice address, System.Action<bool> cb = null);
    }
}