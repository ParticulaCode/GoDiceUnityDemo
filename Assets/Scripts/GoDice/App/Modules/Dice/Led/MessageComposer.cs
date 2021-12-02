using System;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Led
{
    internal static class MessageComposer
    {
        //Firmware will multiple timing by 10 and operates in millisecond
        private const float SecondsScale = 1000f / 10f;

        //Seconds
        private const float BlinkDelay = 0.5f;

        public static readonly sbyte[] Discovery =
            ComposeToggle(
                3,
                Color.green,
                BlinkDelay,
                BlinkDelay,
                true);

        public static readonly sbyte[] ConstantOneRedLed =
            ComposeActivation(Color.black, Color.red);

        public static readonly sbyte[] ConstantTwoRedLed =
            ComposeActivation(Color.red, Color.red);

        public static sbyte[] ComposeToggle(sbyte blinkNumber, Color color, float onDuration,
            float offDuration, bool mixed, sbyte ledsToActivate = WriteProtocol.Led.BothLeds)
        {
            var scaledColor = (Color32) color;
            return new[]
            {
                blinkNumber,
                AdaptTiming(onDuration),
                AdaptTiming(offDuration),
                ToSByteUncheck(scaledColor.r),
                ToSByteUncheck(scaledColor.g),
                ToSByteUncheck(scaledColor.b),
                Convert.ToSByte(mixed),
                ledsToActivate
            };
        }

        private static sbyte ToSByteUncheck(byte b) => unchecked((sbyte) b);

        private static sbyte AdaptTiming(float t) => (sbyte) Mathf.Floor(t * SecondsScale);

        /// RGB for Led1 and RGB for Led2        
        private static sbyte[] ComposeActivation(Color32 color1, Color32 color2) =>
            new[]
            {
                WriteProtocol.Led.Constant,
                ToSByteUncheck(color1.r),
                ToSByteUncheck(color1.g),
                ToSByteUncheck(color1.b),
                ToSByteUncheck(color2.r),
                ToSByteUncheck(color2.g),
                ToSByteUncheck(color2.b)
            };
    }
}