using System;
using System.Collections;
using FrostLib.Containers.Rx;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class BatteryModule : Module
    {
        [Range(0, 100)]
        [SerializeField]
        [ProgressBar(0f, 100f)]
        private float _charge = 100;

        [Range(-5, 5)]
        [Tooltip("% per second")]
        [SerializeField]
        private float _rate;

        [Range(0, 10)]
        [Tooltip("Seconds")]
        [SerializeField]
        private float _responseDelay;

        [Range(0, 5)]
        [Tooltip("Battery level auto brodcast frequency while charging, seconds")]
        [SerializeField]
        private float _broadcastFrequency = 1f;

        public readonly Reactive<bool> PowerOn = new Reactive<bool>();
        public readonly Reactive<bool> IsCharging = new Reactive<bool>();

        private Coroutine _broadcastRoutine;

        private void Awake() => IsCharging.OnChange.AddListener(OnChargingChanged);

        private void OnChargingChanged(bool isCharging)
        {
            SendChargingState();
            HandleBroadcasting(isCharging);
        }

        public void SendChargingState()
        {
            var responce = IsCharging.Value ? Response.ChargingStarted : Response.ChargingStopped;
            var data = Reader.BuildResponse(responce);
            SendData(data);
        }

        private void Update()
        {
            var newValue = _charge + _rate * Time.unscaledDeltaTime;
            _charge = Mathf.Clamp(newValue, 0f, 100f);

            PowerOn.Set(_charge > 0f);
            IsCharging.Set(_rate > 0f);
        }

        private void HandleBroadcasting(bool isCharging)
        {
            if (isCharging)
            {
                if (_broadcastRoutine != null)
                    return;

                _broadcastRoutine = StartCoroutine(BroadcastBattery());
            }
            else
            {
                StopCoroutine(_broadcastRoutine);
                _broadcastRoutine = null;
            }
        }

        private IEnumerator BroadcastBattery()
        {
            var wait = new WaitForSeconds(_broadcastFrequency);
            while (true)
            {
                SendBattery();
                yield return wait;
            }
        }

        public void SendWithDelay() => StartCoroutine(SendDelayed());

        private IEnumerator SendDelayed()
        {
            yield return new WaitForSeconds(_responseDelay);

            SendBattery();
        }

        private void SendBattery()
        {
            var batVal = new[] { (byte) Mathf.RoundToInt(_charge) };
            var data = Reader.BuildResponse(Response.Battery, batVal);
            SendData(data);
        }

        private void OnDestroy()
        {
            PowerOn.Dispose();
            IsCharging.Dispose();
        }
    }
}