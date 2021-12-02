using System.Collections.Generic;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.App.Modules.Dice.Data;
using GoDice.Shared.Data;
using static GoDice.App.Modules.Dice.Messaging.WriteProtocol;

namespace GoDice.App.Modules.Dice.Messaging
{
    public class Writer
    {
        private static class Request
        {
            public static readonly sbyte[] Battery = { WriteProtocol.Battery };
            public static readonly sbyte[] Color = { WriteProtocol.Color.Get };

            public static readonly sbyte[] OpenTapInterrupt = { Tap.Single, Tap.Enable };
            public static readonly sbyte[] CloseTapInterrupt = { Tap.Single, Tap.Disable };

            public static readonly sbyte[] OpenDoubleTapInterrupt = { Tap.Double, Tap.Enable };
            public static readonly sbyte[] CloseDoubleTapInterrupt = { Tap.Double, Tap.Disable };
        }

        private readonly IDevice _device;

        public Writer(IDevice device) => _device = device;

        public void SendInitalizationMessage(sbyte sens, sbyte[] led)
        {
            var message = new List<sbyte>(led.Length + 1) { Initialization, sens };
            message.AddRange(led);
            
            Send(message.ToArray(), OperationType.Initialization);
        }

        public void RequestBatteryCharge() => Send(Request.Battery, OperationType.BatteryRequest);

        public void SetSensitivity(sbyte newSen) =>
            Send(new[] { Sensitivity, newSen }, OperationType.SensitivitySet);

        public void OpenTapInterrupt() =>
            Send(Request.OpenTapInterrupt, OperationType.TapInterruptOpen);

        public void CloseTapInterrupt() =>
            Send(Request.CloseTapInterrupt, OperationType.TapInterruptClose);

        public void OpenDoubleTapInterrupt() => Send(Request.OpenDoubleTapInterrupt,
            OperationType.DoubleTapInterruptOpen);

        public void CloseDoubleTapInterrupt() => Send(Request.CloseDoubleTapInterrupt,
            OperationType.DoubleTapInterruptClose);

        public void RequestColor() => Send(Request.Color, OperationType.ColorRequest);

        public void SendRollDetectionSettings(sbyte[] settings)
        {
            var message = new List<sbyte>(settings.Length + 1) { RollSetting };
            message.AddRange(settings);
            Send(message.ToArray(), OperationType.RollDetectionSettings);
        }

        public void Send(sbyte[] data, OperationType type) => _device.SendData(data, type);
    }
}