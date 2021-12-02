using FrostLib.Extensions;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.App.Modules.Bluetooth.Scan;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    internal class AddDieWindow : MonoBehaviour
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IBluetoothBridge Bridge => Servicer.Get<IBluetoothBridge>();
        private static IEventDispatcher Dispatcher => Servicer.Get<IEventDispatcher>();

        [SerializeField] private GameObject _scanningLabel;
        [SerializeField] private GameObject _refreshLabel;
        [SerializeField] private Button _refreshBtn;
        [SerializeField] private DeviceView _deviceViewPrefab;
        [SerializeField] private Transform _viewsContainer;

        private Sync<IDevice> _sync;

        private void Awake() => _refreshBtn.onClick.AddListener(RefreshScan);

        public void Show()
        {
            gameObject.SetActive(true);
            RefreshScan();
        }

        private void RefreshScan()
        {
            _scanningLabel.SetActive(true);
            _refreshLabel.SetActive(false);
            _refreshBtn.interactable = false;

            ClearDevices();
            Dispatcher.Raise(new NewDevicesScanStartEvent(SetSync));
        }

        private void ClearDevices() => _viewsContainer.DestroyChildren();

        private void SetSync(Sync<IDevice> sync)
        {
            DisposeSync();

            _sync = sync;

            _sync.Collection.OnItemAddedSignal.AddListener(AddDevice);
            _sync.Collection.OnClearSignal.AddListener(ClearDevices);

            _sync.OnScanFinished.AddOnce(OnScanFinished);
        }

        private void AddDevice(IDevice device)
        {
            var view = CreateView();
            view.Initialize(device.Address, device.Name);
            view.OnClick.AddListener(() => OnViewSelected(device));
        }

        private DeviceView CreateView()
        {
            var go = Instantiate(_deviceViewPrefab.gameObject, _viewsContainer);
            go.SetActive(true);
            return go.GetComponent<DeviceView>();
        }

        private void OnViewSelected(IDevice device) =>
            Dispatcher.Raise(new ConnectToDeviceRequestEvent(device, OnConnectionResult));

        private void OnConnectionResult(bool success)
        {
            if (!success)
                return;

            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            Bridge.StopScan();
        }

        private void DisposeSync()
        {
            if (_sync == null)
                return;

            _sync.Collection.OnItemAddedSignal.RemoveListener(AddDevice);
            _sync.Collection.OnClearSignal.RemoveListener(ClearDevices);

            _sync.OnScanFinished?.RemoveListener(OnScanFinished);

            _sync = null;
        }

        private void OnScanFinished()
        {
            _scanningLabel.SetActive(false);
            _refreshLabel.SetActive(true);
            _refreshBtn.interactable = true;
        }
    }
}