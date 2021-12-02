using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Extensions;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Characteristics;
using UnityEngine;

namespace GoDice.App.Modules.Simulation
{
    public class BluetoothBridge : MonoBehaviour, IBluetoothBridge
    {
        [Range(0, 10)]
        [Tooltip("Seconds")]
        [SerializeField]
        private float _ping;

        public IEnumerable<IDie> ConnectedDevices => _connectedDevices;

        private readonly List<BluetoothBridgeDevice> _connectedDevices =
            new List<BluetoothBridgeDevice>();
        private readonly List<BluetoothBridgeDevice> _allDeveices =
            new List<BluetoothBridgeDevice>();

        private Action<string> _onDeviceDisconnected;
        private Action<string, string> _onDeviceDiscovered;
        private Signal<string> _lastConnectionFailureSignal;
        
        private bool _scanning;

        private void Awake()
        {
            var go = gameObject;
            var isRedundantDuplicate = FindObjectsOfType<BluetoothBridge>().Length > 1;
            if (isRedundantDuplicate)
            {
                go.SetActive(false);
                Destroy(go);
                return;
            }

            DontDestroyOnLoad(go);
        }

        void IBluetoothBridge.Initialize(Action onSuccess, Action<string> onError,
            Action<string> onPeripheralDisconnected, Action<string> onLog)
        {
            _onDeviceDisconnected = onPeripheralDisconnected;
            onSuccess();
        }

        void IBluetoothBridge.ChangeDeviceBluetoothState(bool enable)
        {
        }

        void IBluetoothBridge.ConnectToPeripheral(string address, Action<string> connectAction,
            Action<string, string> serviceAction,
            Action<string, string, string> characteristicAction, Signal<string> onFailure)
        {
            _lastConnectionFailureSignal = onFailure;
            
            StartCoroutine(ConnectRoutined(address, connectAction, serviceAction,
                characteristicAction));
        }

        private IEnumerator ConnectRoutined(string address, Action<string> connectAction,
            Action<string, string> serviceAction,
            Action<string, string, string> characteristicAction)
        {
            var device = _allDeveices.First(d => d.Address == address);
            if (!device.IsWorking())
            {
                yield return new WaitForSeconds(_ping);
                
                OnDisconnect(address);
                yield break;
            }

            _connectedDevices.Add(device);
            connectAction(device.Address);

            serviceAction?.Invoke(address, Statics.ServiceUUID);
            yield return new WaitForSeconds(_ping / 2f);

            characteristicAction(address, Statics.ServiceUUID, Statics.ReadCharacteristicUUID);
            yield return new WaitForSeconds(_ping / 2f);

            characteristicAction(address, Statics.ServiceUUID, Statics.WriteCharacteristicUUID);
        }

        private void OnDisconnect(string address)
        {
            _lastConnectionFailureSignal?.Dispatch(address);
            _onDeviceDisconnected?.Invoke(address);
        }

        void IBluetoothBridge.DisconnectPeripheral(string address) =>
            StartCoroutine(DisconnectPeripheralRoutined(address));

        private IEnumerator DisconnectPeripheralRoutined(string address)
        {
            //For reald dice we have a delay before plugin fire a callback 
            yield return new WaitForSeconds(0.1f);

            var device = _connectedDevices.FirstOrDefault((d) => d.Address == address);
            if (device != null)
            {
                device.Disconnect();
                _connectedDevices.Remove(device);
            }

            _onDeviceDisconnected(address);
        }

        void IBluetoothBridge.ScanForPeripherals(Action<string, string> action,
            Action<string, string, int, byte[]> actionAdvertisingInfo)
        {
            _scanning = true;
            _onDeviceDiscovered = action;
            PingAllDevices();
        }

        internal void PingAllDevices()
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                var device = child.gameObject.GetComponent<BluetoothBridgeDevice>();
                if (device != null)
                    device.ProvideSelfToBridge();
            }
        }

        void IBluetoothBridge.StopScan() => _scanning = false;

        void IBluetoothBridge.SubscribeCharacteristicWithDeviceAddress(Characteristic ch,
            Action<string, string> notificationAction, Action<string, string, byte[]> action)
        {
            var device = _connectedDevices.First(d => d.Address == ch.Address);
            device.SubscribeCharacteristic(ch.Id, notificationAction, action);
        }

        void IBluetoothBridge.WriteCharacteristic(Characteristic ch, sbyte[] data, bool withResponse,
            Action<string> onSuccess) =>
            StartCoroutine(WriteCharacteristicRoutined(ch, data, withResponse, onSuccess));

        private IEnumerator WriteCharacteristicRoutined(Characteristic ch, sbyte[] data,
            bool withResponse, Action<string> onSuccess)
        {
            var device = _connectedDevices.First(d => d.Address == ch.Address);
            device.ReceiveData(data);

            yield return new WaitForSeconds(_ping);

            if (withResponse)
                onSuccess?.Invoke(ch.Address);
        }

        internal void DiscoverDevice(BluetoothBridgeDevice device)
        {
            if (!_scanning || _connectedDevices.Contains(device))
                return;

            _allDeveices.AddDistinct(device);
            _onDeviceDiscovered(device.Address, device.Name);
        }

        internal void DisconnectDevice(BluetoothBridgeDevice device)
        {
            if (!_connectedDevices.Contains(device))
                return;

            _connectedDevices.Remove(device);
            OnDisconnect(device.Address);
        }
    }
}