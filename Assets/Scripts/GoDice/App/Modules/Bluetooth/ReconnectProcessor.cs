using System.Collections.Generic;
using System.Linq;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.Shared.EventDispatching.Dispatching;

namespace GoDice.App.Modules.Bluetooth
{
    internal partial class ReconnectProcessor
    {
        private readonly IRunner _operationsRunner;
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IDeviceHolder Holder => Servicer.Get<IDeviceHolder>();
        private static IEventDispatcher Dispatcher => Servicer.Get<IEventDispatcher>();

        private readonly List<Record> _records = new List<Record>();

        public ReconnectProcessor(IRunner operationsRunner) => _operationsRunner = operationsRunner;

        public void Register(IDevice device)
        {
            if (DeviceIsRegistered(device))
                return;

            _records.Add(new Record(device));
            device.OnStateChangedSignal.AddListener(OnDeviceDisconnected);
        }

        private bool DeviceIsRegistered(IDevice device) => RecordByDevice(device) != null;

        private void OnDeviceDisconnected(IDevice device, DeviceState newState)
        {
            var record = RecordByDevice(device);
            if (record == null)
                return;

            switch (newState)
            {
                case DeviceState.Connected:
                    record.Reset();
                    break;
                case DeviceState.Disconnected:
                    _operationsRunner.PruneOperaionsByAddress(device.Address);

                    if (!Holder.Exists(device.Address) || !record.ShouldReconnect())
                        return;

                    Log.Message($"Attempting to reconnect to {device}");
                    record.MarkReconnected();
                    Dispatcher.Raise(new ConnectToDeviceRequestEvent(record.Device));
                    break;
            }
        }

        private Record RecordByDevice(IDevice device) =>
            _records.FirstOrDefault(r => r.Device == device);
    }
}