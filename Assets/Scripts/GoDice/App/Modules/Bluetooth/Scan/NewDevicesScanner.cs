using System.Collections;
using System.Text.RegularExpressions;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Devices;
using UnityEngine;

namespace GoDice.App.Modules.Bluetooth.Scan
{
    internal class NewDevicesScanner : Scanner
    {
        protected override string LogInfo => "New Devices scanner";

        private const int ScanDuration = 15;

        public NewDevicesScanner(IBluetoothBridge bridge, Regex deviceFilter) :
            base(bridge, deviceFilter)
        {
        }

        protected override IEnumerator Scan()
        {
            StartScan();

            yield return new WaitForSecondsRealtime(ScanDuration);

            Stop();
        }

        protected override void OnDeviceFound(string address, string name)
        {
            if (Devices.Exists(address))
                return;

            Bluetooth.Log.Message("New Device > " + address);
            NewDeviceFoundSignal.Dispatch(new Device(address, name));
        }
    }
}