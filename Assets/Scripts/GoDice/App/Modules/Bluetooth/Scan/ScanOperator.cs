using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FrostLib.Collections;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.Shared.EventDispatching.Dispatching;

namespace GoDice.App.Modules.Bluetooth.Scan
{
    internal class ScanOperator : IScanner, IDisposable
    {
        private readonly IEventDispatcher _dispatcher;
        
        private readonly SignaledList<IDevice> _newDevices = new SignaledList<IDevice>();

        private readonly Dictionary<ScanMode, Scanner> _scannersMap =
            new Dictionary<ScanMode, Scanner>();

        private ScanMode _mode = ScanMode.None;

        public ScanOperator(IBluetoothBridge bridge, IEventDispatcher dispatcher, string deviceFilter)
        {
            _dispatcher = dispatcher;
            var filter = new Regex(deviceFilter);

            SetupScanners(bridge, filter);
        }

        private void SetupScanners(IBluetoothBridge bridge, Regex filter)
        {
            var newDevicesScanner = new NewDevicesScanner(bridge, filter);
            newDevicesScanner.NewDeviceFoundSignal.AddListener(_newDevices.Add);
            newDevicesScanner.OnScanStarted.AddListener(ClearNewDevices);
            _scannersMap.Add(ScanMode.NewDevices, newDevicesScanner);
        }

        private void RequestConnectToDevice(IDevice device) =>
            _dispatcher.Raise(new ConnectToDeviceRequestEvent(device));

        private void ClearNewDevices() => _newDevices.Clear();

        Sync<IDevice> IScanner.GetNewDevicesSync() =>
            new Sync<IDevice>(_newDevices, _scannersMap[ScanMode.NewDevices].OnScanStopped);

        void IScanner.SetMode(ScanMode mode)
        {
            _mode = mode;

            //Stop first, to avoid collision with a hardware logic
            foreach (var bind in _scannersMap)
                bind.Value.Stop();

            _scannersMap[_mode].Start();
        }

        void IDisposable.Dispose()
        {
            _scannersMap[ScanMode.ExistingDevices].NewDeviceFoundSignal.RemoveListener(RequestConnectToDevice);
            _scannersMap[ScanMode.AllDevices].NewDeviceFoundSignal.RemoveListener(RequestConnectToDevice);

            var newDevicesScanner = _scannersMap[ScanMode.NewDevices];
            newDevicesScanner.NewDeviceFoundSignal.RemoveListener(_newDevices.Add);
            newDevicesScanner.OnScanStarted.RemoveListener(_newDevices.Clear);

            ClearNewDevices();

            foreach (var bind in _scannersMap)
                bind.Value.Stop();
        }
    }
}