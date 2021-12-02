using System;
using FrostLib.Signals.impl;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class Transmitter : MonoBehaviour
    {
        private enum DeviceState
        {
            Transmitting,
            Silent
        }

        [HideLabel]
        [SerializeField]
        [EnumToggleButtons]
        [OnValueChanged(nameof(DeviceStateValueChanged))]
        private DeviceState _deviceState = DeviceState.Transmitting;

        public bool IsOn => _deviceState == DeviceState.Transmitting;

        public Signal<bool> OnStateChanged = new Signal<bool>();

        private void DeviceStateValueChanged() => OnStateChanged.Dispatch(IsOn);

        public void Activate() => SetState(DeviceState.Transmitting);

        public void Deactivate() => SetState(DeviceState.Silent);

        private void SetState(DeviceState newState)
        {
            _deviceState = newState;
            DeviceStateValueChanged();
        }

        private void OnDestroy() => OnStateChanged.ClearListeners();
    }
}