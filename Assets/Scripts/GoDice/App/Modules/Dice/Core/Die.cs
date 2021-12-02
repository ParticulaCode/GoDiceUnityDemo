using System;
using FrostLib.Containers;
using FrostLib.Containers.Rx;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Dice.Connectors;
using GoDice.App.Modules.Dice.Data;
using GoDice.App.Modules.Dice.Events;
using GoDice.App.Modules.Dice.Led;
using GoDice.App.Modules.Dice.Messaging;
using GoDice.App.Modules.Dice.Shells;
using GoDice.Shared.Data;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.Events.Dice;
using GoDice.Utils;

namespace GoDice.App.Modules.Dice.Core
{
    internal class Die : IDie, IDisposable
    {
        public Guid Id { get; }
        public Guid DeviceId => _device.Id;
        public string Address => _device.Address;

        public ILedController Led { get; }

        public ReadonlyReactive<string> Name { get; }
        public ReadonlyReactive<int> Value { get; }
        public ReadonlyReactive<bool> IsConnected { get; }
        public ReadonlyReactive<bool> IsCharging { get; }
        public ReadonlyReactive<float> Battery { get; }
        public ReadonlyReactive<ShellType> Shell { get; }
        public ReadonlyReactive<ColorType> Color { get; }

        private static IEventDispatcher Dispatcher => ServiceLocator.Instance.Get<IEventDispatcher>();

        private readonly Reactive<string> _name = new Reactive<string>();
        private readonly Reactive<int> _value = new Reactive<int>();
        private readonly Reactive<bool> _isConnected = new Reactive<bool>();
        private readonly Reactive<bool> _isCharging = new Reactive<bool>();
        private readonly Reactive<float> _battery = new Reactive<float>();
        private readonly Reactive<ShellType> _shell = new Reactive<ShellType>(ShellType.D6);
        private readonly Reactive<ColorType> _color = new Reactive<ColorType>();

        private readonly IDevice _device;
        private readonly IShellController _shellController;
        private readonly ConnectionBroadcaster _connectionBroadcaster;
        private readonly Writer _writer;
        private readonly Reader _reader;
        private readonly IActivable _singleTapListener;
        private readonly IActivable _doubleTapListener;

        public Die(Data.DieData data, IDevice device, ILedController led,
            IShellController shellController, Reader reader, Writer writer)
        {
            Name = _name.ToReadonly();
            Value = _value.ToReadonly();
            IsConnected = _isConnected.ToReadonly();
            IsCharging = _isCharging.ToReadonly();
            Shell = _shell.ToReadonly();
            Battery = _battery.ToReadonly();
            Color = _color.ToReadonly();

            Id = data.Id;
            SetName(data.Name);
            SetValue(1);
            InitShell(data);
            SetColor(data.Color);

            _device = device;
            Led = led;

            _shellController = shellController;
            _shellController.SetShell(Shell);

            _reader = reader;
            _writer = writer;

            _connectionBroadcaster = new ConnectionBroadcaster(this, IsConnected.OnChange);

            _singleTapListener =
                new BinaryActivator(_writer.OpenTapInterrupt, _writer.CloseTapInterrupt);

            _doubleTapListener = new BinaryActivator(_writer.OpenDoubleTapInterrupt,
                _writer.CloseDoubleTapInterrupt);

            _device.OnStateChangedSignal.AddListener(OnDeviceStateChanged);
            _device.OnDataReceivedSignal.AddListener(OnDataReceived);

            _isCharging.OnChange.AddListener(isOn =>
                Dispatcher.Raise(new DieChargingStateChangedEvent(this, isOn)));
        }

        public void SetName(string name) => _name.Set(name);

        private void SetValue(int newValue) => _value.Set(newValue);

        private void InitShell(Data.DieData data)
        {
            if (data.Shell != ShellType.None)
                SetShell(data.Shell);

            Shell.OnChange.AddListener(ShellChanged);
        }

        private void OnDeviceStateChanged(IDevice arg1, DeviceState arg2) => CheckConnection();

        public void CheckConnection() => _isConnected.Set(_device.State == DeviceState.Connected);

        private void OnDataReceived(byte[] data)
        {
            switch (_reader.Read(data))
            {
                case Response.Undefined:
                    break;
                case Response.Battery:
                    UpdateBattery();
                    break;
                case Response.Roll:
                    StartRolling();
                    break;
                case Response.RollEnd:
                    EndRoll();
                    break;
                case Response.FakeStable:
                case Response.MoveStable:
                case Response.TiltStable:
                    BecomeStable();
                    break;
                case Response.Tap:
                    HandleTap();
                    break;
                case Response.DoubleTap:
                    HandleDoubleTap();
                    break;
                case Response.ChargingStarted:
                    SetCharging(true);
                    break;
                case Response.ChargingStopped:
                    SetCharging(false);
                    break;
                case Response.Color:
                    SetColor(_reader.Color);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Battery

        public void RequestBatteryCharge() => _writer.RequestBatteryCharge();

        private void UpdateBattery() => _battery.Set(_reader.Battery);

        private void SetCharging(bool isOn) => _isCharging.Set(isOn);

        #endregion

        #region Rolls

        private void StartRolling() => Dispatcher.Raise(new DieStartedRollEvent(Id));

        private void EndRoll()
        {
            SetValue(_shellController.GetValue(_reader.Axis));
            Dispatcher.Raise(new DieEndedRollEvent(Id, Value));
        }

        private void BecomeStable()
        {
            SetValue(_shellController.GetValue(_reader.Axis));
            Dispatcher.Raise(new DieStableEvent(Id, Value));
        }

        #endregion

        #region Taps

        private void HandleTap() => Dispatcher.Raise(new DieTapEvent(Id));

        private void HandleDoubleTap() => Dispatcher.Raise(new DieDoubleTapEvent(Id));

        public void ExpectTap() => _singleTapListener.Activate();

        public void IgnoreTap() => _singleTapListener.Deactivate();

        public void ExpectDoubleTap() => _doubleTapListener.Activate();

        public void IgnoreDoubleTap() => _doubleTapListener.Deactivate();

        #endregion

        #region Shell

        private void ShellChanged(ShellType type)
        {
            _shellController.SetShell(type);
            AssignSensitivityByCurrentShell();
            Dispatcher.Raise(new DieShellChangedEvent(this));
        }

        public void SetShell(ShellType shellType) => _shell.Set(shellType);

        private void AssignSensitivityByCurrentShell() => _writer.SetSensitivity(GetSensitivity());

        private sbyte GetSensitivity() => _shellController.GetSensitivity();

        #endregion

        #region Color

        private void SetColor(ColorType color) => _color.Set(color);

        public void RequestColor() => _writer.RequestColor();

        #endregion

        public void SendInitializationMessage()
        {
            var sens = GetSensitivity();
            var ledMessage = Led.GetMessage(ToggleMode.Discover);
            _writer.SendInitalizationMessage(sens, ledMessage);
        }

        public void Dispose()
        {
            _device.OnStateChangedSignal.RemoveListener(OnDeviceStateChanged);
            _device.OnDataReceivedSignal.RemoveListener(OnDataReceived);

            _connectionBroadcaster.Dispose();

            _name.Dispose();
            _isConnected.Dispose();
            _isCharging.Dispose();
            _shell.Dispose();
            _battery.Dispose();
        }

        public void SendRollDetectionParams(sbyte[] settings) =>
            _writer.SendRollDetectionSettings(settings);

        public override string ToString() =>
            $"Die (Name = {Colorizer.AsDie(Name)}, Color = {Color}, Shell = {Shell}, Id = {Id}, {_device})";
    }
}