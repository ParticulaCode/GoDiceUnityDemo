#if USE_BLE_PLUGIN
using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Characteristics;

namespace GoDice.App.Modules.Bluetooth.Bridge
{
    internal class BridgeToLib : IBluetoothBridge
    {
        private Action<string> _onDisconnect;
        private Signal<string> _lastConnectionFailureSignal;

        private bool _scanIsRunning;
        
        void IBluetoothBridge.Initialize(Action onSuccess, Action<string> onError,
            Action<string> onPeripheralDisconnected, Action<string> onLog)
        {
            _onDisconnect = onPeripheralDisconnected;
            BluetoothLEHardwareInterface.Initialize(true, false, onSuccess, onError, onLog);
        }

        void IBluetoothBridge.ChangeDeviceBluetoothState(bool enable) =>
            BluetoothLEHardwareInterface.BluetoothEnable(enable);

        void IBluetoothBridge.ConnectToPeripheral(string name, Action<string> connectAction,
            Action<string, string> serviceAction, Action<string, string, string> characteristicAction,
            Signal<string> onFailure)
        {
            _lastConnectionFailureSignal = onFailure;

            BluetoothLEHardwareInterface.ConnectToPeripheral(name, connectAction, serviceAction,
                characteristicAction, OnDisconnect);
        }

        private void OnDisconnect(string address)
        {
            _lastConnectionFailureSignal?.Dispatch(address);
            _onDisconnect?.Invoke(address);
        }

        void IBluetoothBridge.DisconnectPeripheral(string address) =>
            BluetoothLEHardwareInterface.DisconnectPeripheral(address, null);

        void IBluetoothBridge.ScanForPeripherals(Action<string, string> action,
            Action<string, string, int, byte[]> actionAdvertisingInfo)
        {
            if(_scanIsRunning)
                return;
            
            try
            {
                _scanIsRunning = true;
                BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, action,
                    actionAdvertisingInfo, false, false);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("android.bluetooth.le.BluetoothLeScanner.startScan"))
                {
                    Log.Message($"Seems like bluetooth is disabled. Got an error from android: {e.Message}, \n{e.StackTrace}");
                    return;
                }

                UnityEngine.Debug.LogException(e);
            }
        }

        void IBluetoothBridge.StopScan()
        {
            //BLE plugin will throw a null ref error if Stop is called while scan is not running
            if(!_scanIsRunning)
                return;
            
            try
            {
                _scanIsRunning = false;
                BluetoothLEHardwareInterface.StopScan();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        void IBluetoothBridge.SubscribeCharacteristicWithDeviceAddress(Characteristic ch,
            Action<string, string> notificationAction, Action<string, string, byte[]> action)
            => BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(ch.Address,
                ch.Service, ch.Id, notificationAction, action);

        ///OnSucess will return device address
        void IBluetoothBridge.WriteCharacteristic(Characteristic ch, sbyte[] data, bool withReponse,
            Action<string> onSuccess)
            => BluetoothLEHardwareInterface.WriteCharacteristic(ch.Address, ch.Service, ch.Id, data,
                data.Length, withReponse, onSuccess);
    }
}
#endif