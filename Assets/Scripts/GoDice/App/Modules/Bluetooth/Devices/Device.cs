using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Characteristics;
using GoDice.App.Modules.Bluetooth.Devices.Data;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.Utils;

namespace GoDice.App.Modules.Bluetooth.Devices
{
    internal class Device : IDevice, IDisposable
    {
        public Guid Id => _info.Id;
        public string Address => _info.Address;
        public string Name => _info.Name;

        public DeviceState State
        {
            get => _state;
            set
            {
                var prevState = _state;
                _state = value;

                if (_state == DeviceState.Disconnected)
                {
                    AttachReader(null);
                    AttachWriter(null);
                }

                if (prevState == _state)
                    return;

                Log.Message($"[{Colorizer.AsAddress(Address)}] {prevState} --> {_state} ");
                OnStateChangedSignal.Dispatch(this, _state);
            }
        }

        private DeviceState _state = DeviceState.Disconnected;

        public Signal<IDevice, DeviceState> OnStateChangedSignal { get; } =
            new Signal<IDevice, DeviceState>();

        public Signal<byte[]> OnDataReceivedSignal { get; } = new Signal<byte[]>();

        private readonly Info _info;

        private Reader _reader;
        private Writer _writer;

        public Device(Info info) => _info = info;

        public Device(string address, string name) :
            this(new Info()
            {
                Address = address,
                Name = name,
                Id = Guid.NewGuid()
            })
        {
        }

        public void AttachReader(Reader reader)
        {
            if (_reader == reader)
                return;

            Log.Message($"{this}: Attach new characteristic reader: {reader}. Previous: {_reader}");

            _reader?.OnDataSignal.RemoveListener(OnDataReceived);
            _reader = reader;
            _reader?.OnDataSignal.AddListener(OnDataReceived);
        }

        private void OnDataReceived(byte[] data) => OnDataReceivedSignal.Dispatch(data);

        public void AttachWriter(Writer writer) => _writer = writer;

        public void SendData(sbyte[] data, OperationType type) => _writer?.Send(data, type);

        public void Dispose()
        {
            State = DeviceState.Disconnected;
            
            _reader?.OnDataSignal.RemoveListener(OnDataReceived);

            OnStateChangedSignal.ClearListeners();
            OnDataReceivedSignal.ClearListeners();
        }

        public override string ToString() => $"Device (Name = {Colorizer.AsDevice(Name)}, Address = {Colorizer.AsAddress(Address)}, State = {State})";
    }
}