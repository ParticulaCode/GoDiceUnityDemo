using FrostLib.Commands;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.Utils;

namespace GoDice.App.Modules.Bluetooth.Bridge
{
    internal class SetupBridgeCommand : ICommand<IBluetoothBridge>
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IDeviceHolder Holder => Servicer.Get<IDeviceHolder>();

        private IBluetoothBridge _bridge;

        public IBluetoothBridge Execute()
        {
            var providedBridge = Servicer.Get<IBluetoothBridge>();
            _bridge = providedBridge
#if USE_BLE_PLUGIN
                      ?? new BridgeToLib();
#else
                ;
#endif
            _bridge.Initialize(OnSuccess, OnError, OnPerpheralDisconnected, Log.Operation);

            return _bridge;
        }

        private void OnSuccess() => Log.Message(
            $"Bluetooth bridge initilized: {_bridge.GetType().UnderlyingSystemType.Name}");

        private void OnError(string error)
        {
            Log.Message(Colorizer.AsError($"==== Bt Error: {error} ===="));

            if (error.Contains("Bluetooth LE Not Enabled"))
                _bridge.ChangeDeviceBluetoothState(true);
        }

        private static void OnPerpheralDisconnected(string address)
        {
            Log.Message($"{Colorizer.AsError("Device disconnected:")} {Colorizer.AsAddress(address)}.");

            var device = (Device) Holder.Get(address);
            ProcessConnectionLossIfDeviceExists(device);
        }

        private static void ProcessConnectionLossIfDeviceExists(Device device)
        {
            if (device == null)
                return;

            device.State = DeviceState.Disconnected;
        }
    }
}