using System;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.Shared.EventDispatching.Dispatching;

namespace GoDice.App.Modules.Bluetooth
{
    internal class ConnestionEstablisher : IDeviceConnector
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Servicer.Get<IEventDispatcher>();
        private static IDeviceHolder Holder => Servicer.Get<IDeviceHolder>();

        private readonly IRunner _runner;
        private readonly ReconnectProcessor _reconnector;

        public ConnestionEstablisher(IRunner runner, ReconnectProcessor reconnector)
        {
            _runner = runner;
            _reconnector = reconnector;
        }

        void IDeviceConnector.Connect(IDevice device, Action<bool> cb)
        {
            if (device.State == DeviceState.Connected)
                return;

            var establisher = new ConnectionOperation(_runner, (Device) device,
                (success, connectedDevice) => OnConnectionEstablished(success, connectedDevice, cb));

            _runner.Schedule(establisher);
        }

        private void OnConnectionEstablished(bool success, IDevice connectedDevice,
            Action<bool> cb)
        {
            if (success)
            {
                if (!Holder.Exists(connectedDevice.Address))
                    Holder.Add(connectedDevice);

                _reconnector.Register(connectedDevice);
                Dispatcher.Raise(new NewDeviceConnectedEvent(connectedDevice));
            }

            cb?.Invoke(success);
        }
    }
}