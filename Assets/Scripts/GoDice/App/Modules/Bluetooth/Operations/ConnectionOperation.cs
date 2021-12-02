using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Characteristics;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.Utils;

// ReSharper disable InconsistentNaming
namespace GoDice.App.Modules.Bluetooth.Operations
{
    // From my extensive tests, a device will fail all write operations up 
    // untill the point where a notification is received in the read characteristic.
    // The notification just contains the UUID of the read characteristic.
    // So thats why we have to wait for both characteristics and the notification 
    // from the read characteristic in order to deem the device readable and writable
    internal class ConnectionOperation : IOperation
    {
        public bool IsDone { get; private set; }

        private const OperationType _type = OperationType.Connection;

        private readonly IRunner _runner;
        private readonly Action<bool, IDevice> _callback;
        private readonly Device _device;
        private readonly Signal<string> _failureSignal = new Signal<string>();

        private IBluetoothBridge _bridge;
        private Reader _reader;
        private Writer _writer;
        private bool _gotNotification;

        public ConnectionOperation(IRunner runner, Device device, Action<bool, IDevice> cb = null)
        {
            _runner = runner;
            _device = device;
            _callback = cb;
        }

        public bool CanBePruned(string address) => false;

        public void Perform(IBluetoothBridge bridge)
        {
            _bridge = bridge;
            _device.State = DeviceState.Connecting;
            _failureSignal.AddOnce(OnFailure);

            _bridge.ConnectToPeripheral(_device.Address, OnConnectionStarted, null, OnCharecteristic,
                _failureSignal);
        }

        private void OnFailure(string address)
        {
            if (address != _device.Address)
                return;

            Abort();
        }

        public void Abort()
        {
            Log.Message(
                Colorizer.AsError($"Failed to establish connection with {_device}. Aborting."));

            Finish(false);
        }

        private void Finish(bool isSuccess)
        {
            IsDone = true;
            _device.State = isSuccess ? DeviceState.Connected : DeviceState.Disconnected;
            _callback?.Invoke(isSuccess, isSuccess ? _device : null);
            _failureSignal.ClearListeners();
        }

        private void OnConnectionStarted(string address)
        {
            Log.Message($"Establishing connection to {_device}");
            CheckState();
        }

        private void CheckState()
        {
            if (IsDone || _reader == null || _writer == null || !_gotNotification)
                return;

            Log.Message($"Connection established with {_device}");
            Finish(true);
        }

        private void OnCharecteristic(string address, string serviceUUID, string characteristicUUID)
        {
            if (serviceUUID.ToUpper() != Statics.ServiceUUID || address != _device.Address)
                return;

            Log.Message($"OnCharecteristic: address: {Colorizer.AsAddress(address)}");

            var upper = characteristicUUID.ToUpper();
            var ch = new Characteristic(address, serviceUUID, characteristicUUID);
            if (upper == Statics.ReadCharacteristicUUID)
            {
                Log.Message($"\t=> Found read characteristic for {_device}");
                _reader = new Reader(ch);
                _reader.OnNotificationSignal.AddOnce(OnNotification);
                _device.AttachReader(_reader);

                _bridge.SubscribeCharacteristicWithDeviceAddress(ch, _reader.OnNotification,
                    _reader.OnReceive);
            }
            else if (upper == Statics.WriteCharacteristicUUID)
            {
                Log.Message($"\t=> Found write characteristic for {_device}");
                _writer = new Writer(ch, _runner);
                _device.AttachWriter(_writer);
            }

            CheckState();
        }

        private void OnNotification(string notification)
        {
            Log.Message($"Notification recieved: {_device}");
            _gotNotification = true;
            CheckState();
        }

        #region Object overrides

        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionOperation other))
                return false;

            return Equals(other);
        }

        private bool Equals(ConnectionOperation other) => _device.Equals(other._device);

        public override int GetHashCode() => _device != null ? _device.GetHashCode() : 0;

        public override string ToString() => Colorizer.AsOperation(_device.Address, _type.ToString());

        #endregion
    }
}