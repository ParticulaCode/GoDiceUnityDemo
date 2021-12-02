using System;
using System.Collections;
using System.Linq;
using GoDice.App.Modules.Bluetooth;
using GoDice.App.Modules.Dice.Messaging;
using GoDice.App.Modules.Simulation.Modules;
using UnityEngine;

namespace GoDice.App.Modules.Simulation
{
    internal class BluetoothBridgeDevice : MonoBehaviour, IDie
    {
        [SerializeField] private string _address;
        [SerializeField] private string _name;

        [Range(0, 10)]
        [Tooltip("Seconds")]
        [SerializeField]
        private float _discoveryDelay;

        public string Address => _address;
        public string Name => _name;
        
        private Action<string, string> _onNotification;
        private Action<string, string, byte[]> _onData;

        private RollModule _rollModule;
        private LedModule _led;
        private BatteryModule _battery;
        private Transmitter _transmitter;
        private ColorModule _colorModule;

        private bool _initialized;
        private TapModule _singleTapModule;
        private DoubleTapModule _doubleTapModule;

        private void Start()
        {
            InitializeModules();
            ProvideSelfToBridge();
            SetName();
        }

        public void ProvideSelfToBridge()
        {
            if (!IsWorking())
                return;

            StartCoroutine(ProvideSelfToBridgeRoutined());
        }

        public bool IsWorking() => _transmitter != null && _transmitter.IsOn;

        private IEnumerator ProvideSelfToBridgeRoutined()
        {
            yield return new WaitForSeconds(_discoveryDelay);

            GetComponentInParent<BluetoothBridge>().DiscoverDevice(this);
        }

        private void InitializeModules()
        {
            if (_initialized)
                return;

            _initialized = true;

            _transmitter = GetComponent<Transmitter>();
            _transmitter.OnStateChanged.AddListener(DeviceTransmissionStateChanged);

            _battery = GetComponent<BatteryModule>();
            _battery.Initialize(SendData);
            _battery.PowerOn.OnChange.AddListener(isOn =>
            {
                if (isOn)
                    _transmitter.Activate();
                else
                    _transmitter.Deactivate();
            });

            _led = GetComponent<LedModule>();
            _led.Initialize(SendData);

            _rollModule = GetComponent<RollModule>();
            _rollModule.Initialize(SendData);

            _colorModule = GetComponent<ColorModule>();
            _colorModule.Initialize(SendData);

            _singleTapModule = GetComponent<TapModule>();
            _singleTapModule?.Initialize(SendData);

            _doubleTapModule = GetComponent<DoubleTapModule>();
            _doubleTapModule?.Initialize(SendData);
        }

        private void SendData(byte[] bytes) =>
            _onData?.Invoke(_address, Statics.ReadCharacteristicUUID, bytes);

        [ContextMenu("Set Name")] public void SetName() => name = _address;

        public void SubscribeCharacteristic(string UUID, Action<string, string> onNotification,
            Action<string, string, byte[]> onData)
        {
            if (UUID != Statics.ReadCharacteristicUUID)
                return;

            _onNotification = onNotification;
            _onData = onData;
            InitializeModules();

            //In a real device we recieve notification after read and write characteristic.
            //It means that device is ready for a further communication
            _onNotification(_address, Statics.ReadCharacteristicUUID);
        }

        public void ReceiveData(sbyte[] data)
        {
            switch (data[0])
            {
                case WriteProtocol.Battery:
                    _battery.SendWithDelay();
                    break;
                case WriteProtocol.Led.Off:
                case WriteProtocol.Led.Toggle:
                case WriteProtocol.Led.Constant:
                    _led.Parse(data);
                    break;
                case WriteProtocol.Color.Get:
                    _colorModule.Send();
                    break;
                case WriteProtocol.Initialization:
                    //ommit sensitivity
                    _led.Parse(data.Skip(2).ToArray().Prepend(WriteProtocol.Led.Toggle).ToArray());
                    
                    _battery.SendWithDelay();
                    
                    if(_battery.IsCharging)
                        _battery.SendChargingState();
                    
                    break;
            }
        }

        private void DeviceTransmissionStateChanged(bool isOn)
        {
            if (isOn)
            {
                ProvideSelfToBridge();
            }
            else
            {
                GetComponentInParent<BluetoothBridge>().DisconnectDevice(this);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            _onNotification = null;
            _onData = null;
        }

        // ReSharper disable Unity.NoNullPropagation
        void IDie.Roll() => _rollModule?.Roll();

        void IDie.Roll(int value) => _rollModule?.Roll(value);

        void IDie.Rotate() => _rollModule?.Rotate();
        
        void IDie.Tap() => _singleTapModule?.Tap();

        void IDie.DoubleTap() => _doubleTapModule?.Tap();
        // ReSharper restore Unity.NoNullPropagation
    }
}