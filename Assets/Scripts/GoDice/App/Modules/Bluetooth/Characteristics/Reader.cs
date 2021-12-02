using System.Collections.Generic;
using System.Text;
using FrostLib.Signals.impl;
using GoDice.Utils;

namespace GoDice.App.Modules.Bluetooth.Characteristics
{
    internal class Reader
    {
        public readonly Signal<byte[]> OnDataSignal = new Signal<byte[]>();
        public readonly Signal<string> OnNotificationSignal = new Signal<string>();

        private readonly Characteristic _characteristic;

        public Reader(Characteristic characteristic) => _characteristic = characteristic;

        public void OnNotification(string address, string notification)
        {
            if (_characteristic.Address != address)
                return;

            OnNotificationSignal?.Dispatch(notification);
        }

        public void OnReceive(string address, string characteristic, byte[] data)
        {
            if (_characteristic.Address != address)
                return;

            if (data.Length == 0)
            {
                LogMessage("Receive ignored! Data is empty");
                return;
            }

            var s = Encoding.UTF8.GetString(data);
            LogMessage($"Data received > {PrintBytes(data)} ({s})");
            OnDataSignal?.Dispatch(data);
        }

        private static string PrintBytes(IReadOnlyList<byte> bytes)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Count; ++i)
            {
                if (i > 0)
                    builder.Append(" ");
                if (bytes[i] < 10)
                {
                    builder.Append("0");
                    builder.Append(bytes[i]);
                }
                else
                {
                    builder.Append(bytes[i]);
                }
            }

            return builder.ToString();
        }

        private void LogMessage(string text) =>
            Log.Operation($"[{Colorizer.AsAddress(_characteristic.Address)}] {text}");
    }
}