using System.Collections.Generic;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Led
{
    internal class LedController : ILedController
    {
        private static readonly Dictionary<ToggleMode, sbyte[]> Messages =
            new Dictionary<ToggleMode, sbyte[]>()
            {
                { ToggleMode.Discover, MessageComposer.Discovery },
            };

        private static readonly Dictionary<OpenMode, sbyte[]> OpenMessages =
            new Dictionary<OpenMode, sbyte[]>()
            {
                { OpenMode.Single, MessageComposer.ConstantOneRedLed },
                { OpenMode.Dual, MessageComposer.ConstantTwoRedLed }
            };

        private readonly Writer _writer;

        public LedController(Writer writer) => _writer = writer;

        public void Blink(ToggleMode mode)
        {
            var message = new List<sbyte>() { WriteProtocol.Led.Toggle };
            message.AddRange(GetMessage(mode));

            _writer.Send(message.ToArray(), OperationType.LedToggle);
        }

        public void Blink(int blinksAmount, Color color, float onDuration,
            float offDuration, bool isMixed)
        {
            var message = new List<sbyte>() { WriteProtocol.Led.Toggle };
            message.AddRange(
                MessageComposer.ComposeToggle(
                    (sbyte) blinksAmount, 
                    color,
                    onDuration, 
                    offDuration,
                    isMixed));

            _writer.Send(message.ToArray(), OperationType.LedToggle);
        }

        public void CloseAllLeds() =>
            _writer.Send(new[] { WriteProtocol.Led.Off }, OperationType.LedOff);

        public void OpenLed(OpenMode mode) =>
            _writer.Send(OpenMessages[mode], OperationType.LedConstant);

        public sbyte[] GetMessage(ToggleMode mode) => Messages[mode];
    }
}