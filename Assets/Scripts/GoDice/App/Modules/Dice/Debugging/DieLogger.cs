using System;
using GoDice.App.Modules.Dice.Core;
using GoDice.Shared.Data;
using GoDice.Shared.EventDispatching.Events;
using static GoDice.App.Modules.Dice.Debugging.Log;

namespace GoDice.App.Modules.Dice.Debugging
{
    internal class DieLogger : IDisposable
    {
        public readonly Die Die;

        public DieLogger(Die die)
        {
            Die = die;

            Die.IsCharging.OnChange.AddListener(OnChargingChanged);
            Die.Color.OnChange.AddListener(OnColorChanged);
            Die.IsConnected.OnChange.AddListener(OnConnectionChanged);
        }

        private void LogDie(string message) => Message($"{Die}:\n\t=> {message}");

        private void OnChargingChanged(bool isOn) => LogDie($"IsCharging: {isOn}");

        private void OnColorChanged(ColorType newColor) => LogDie($"SetColor: {newColor}");

        private void OnConnectionChanged(bool isOn) => LogDie(isOn ? "Connected" : "Disconnected");

        public void LogEvent(EventType evType)
        {
            switch (evType)
            {
                case EventType.DieStartedRoll:
                    LogDie("Roll started");
                    break;
                case EventType.DieEndedRoll:
                    LogDie($"Roll ended with {Die.Value}");
                    break;
                case EventType.DieStable:
                    LogDie($"Stable {Die.Value}");
                    break;
                case EventType.DieRotated:
                    LogDie($"Rotated to {Die.Value}");
                    break;
            }
        }

        public void Dispose()
        {
            Die.IsCharging.OnChange.RemoveListener(OnChargingChanged);
            Die.Color.OnChange.RemoveListener(OnColorChanged);
            Die.IsConnected.OnChange.RemoveListener(OnConnectionChanged);
        }
    }
}