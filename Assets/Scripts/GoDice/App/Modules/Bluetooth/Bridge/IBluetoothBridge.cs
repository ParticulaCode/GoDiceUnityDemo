using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Characteristics;

namespace GoDice.App.Modules.Bluetooth.Bridge
{
    public interface IBluetoothBridge
    {
        void Initialize(Action onSuccess, Action<string> onError,
            Action<string> onPeripheralDisconnected, Action<string> onLog);

        void ChangeDeviceBluetoothState(bool enable);

        void ConnectToPeripheral(string address, Action<string> connectAction,
            Action<string, string> serviceAction, Action<string, string, string> characteristicAction,
            Signal<string> onFailure);

        void DisconnectPeripheral(string address);

        void ScanForPeripherals(Action<string, string> action,
            Action<string, string, int, byte[]> actionAdvertisingInfo = null);

        void StopScan();

        void SubscribeCharacteristicWithDeviceAddress(Characteristic characteristic,
            Action<string, string> notificationAction = null,
            Action<string, string, byte[]> action = null);

        void WriteCharacteristic(Characteristic characteristic, sbyte[] data, bool withResponse = false,
            Action<string> onSuccess = null);
    }
}