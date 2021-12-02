using System.Collections;
using System.Text.RegularExpressions;
using FrostLib.Coroutines;
using FrostLib.Services;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Devices;
using UnityEngine;

namespace GoDice.App.Modules.Bluetooth.Scan
{
    internal abstract class Scanner
    {
        private bool IsRunning => _routine != null;
        
        public readonly Signal OnScanStarted = new Signal();
        public readonly Signal OnScanStopped = new Signal();
        public readonly Signal<IDevice> NewDeviceFoundSignal = new Signal<IDevice>();
        
        protected abstract string LogInfo { get; }
        
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IRoutineRunner Runner => Servicer.Get<IRoutineRunner>();
        protected static IDeviceHolder Devices => Servicer.Get<IDeviceHolder>();

        private readonly IBluetoothBridge _bridge;
        private readonly Regex _deviceFilter;

        private Coroutine _routine;

        protected Scanner(IBluetoothBridge bridge, Regex deviceFilter)
        {
            _bridge = bridge;
            _deviceFilter = deviceFilter;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            _routine = Runner.StartRoutine(Scan());
        }

        protected abstract IEnumerator Scan();

        public void Stop()
        {
            if (!IsRunning)
                return;

            Runner.StopRoutine(_routine);
            _routine = null;
            StopScan();
        }

        protected void StopScan()
        {
            Log("scan ended");
            
            _bridge.StopScan();
            OnScanStopped.Dispatch();
        }

        protected void StartScan()
        {
            Log("scan started");
            
            OnScanStarted.Dispatch();
            _bridge.ScanForPeripherals(OnReceiveDeviceAddress);
        }
        
        protected void Log(string str) => Bluetooth.Log.Message($"{LogInfo}: {str}");

        private void OnReceiveDeviceAddress(string address, string name)
        {
            if (!IsGoDiceDevice(name))
                return;

            OnDeviceFound(address, name);
        }

        private bool IsGoDiceDevice(string name) => _deviceFilter.IsMatch(name);

        protected abstract void OnDeviceFound(string address, string name);
    }
}