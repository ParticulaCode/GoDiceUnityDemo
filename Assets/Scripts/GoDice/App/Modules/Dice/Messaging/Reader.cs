using System;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Extensions;
using GoDice.App.Modules.Dice.Data;
using GoDice.Shared.Data;
using GoDice.Utils;
using UnityEngine;
using static System.Text.Encoding;

namespace GoDice.App.Modules.Dice.Messaging
{
    public class Reader
    {
        public float Battery { get; private set; }
        public Vector3Int Axis { get; private set; }
        public ColorType Color { get; private set; }

        private const float BatteryScale = 100f;
        private static readonly int ColorsCount = Enum<ColorType>.Count;

        private static readonly Dictionary<string, Response> StableVariants = new Dictionary<string, Response>()
        {
            {ReadProtocol.Stable, Response.RollEnd},
            {ReadProtocol.FakeStable, Response.FakeStable},
            {ReadProtocol.TiltStable, Response.TiltStable},
            {ReadProtocol.MoveStable, Response.MoveStable},
        };
 
        public Response Read(byte[] data)
        {
            var str = UTF8.GetString(data);

            switch (str)
            {
                case ReadProtocol.Roll:
                    return Response.Roll;
                case ReadProtocol.Tap:
                    return Response.Tap;
                case ReadProtocol.DoubleTap:
                    return Response.DoubleTap;
            }

            if (str.StartsWith(ReadProtocol.Battery))
            {
                Battery = data[3] / BatteryScale;
                return Response.Battery;
            }

            if (str.StartsWith(ReadProtocol.Charging))
            {
                var valuePos = ReadProtocol.Charging.Length;
                return data[valuePos] == 0 ? Response.ChargingStopped : Response.ChargingStarted;
            }

            foreach (var variant in StableVariants.Where(variant => str.StartsWith(variant.Key)))
            {
                ToAxis(data, variant.Key.Length);
                return variant.Value;
            }

            if (str.StartsWith(ReadProtocol.Color))
            {
                var colorNum = data[3] + 1;
                Color = colorNum < ColorsCount ? (ColorType) colorNum  : ColorType.None;
                return Response.Color;
            }

            UnityEngine.Debug.LogError($"{Colorizer.AsError("Undefined string:")} {str}");
            
            return Response.Undefined;
        }

        private void ToAxis(IReadOnlyList<byte> data, int pos) => Axis = new Vector3Int((sbyte) data[pos], (sbyte) data[pos + 1], (sbyte) data[pos + 2]);

        public byte[] BuildResponse(Response response, IEnumerable<byte> data = null)
        {
            return response switch
            {
                Response.Undefined => new byte[0],
                Response.Battery => WithData(ReadProtocol.Battery, data),
                Response.Roll => GetBytes(ReadProtocol.Roll),
                Response.RollEnd => GetBytes(ReadProtocol.Stable),
                Response.FakeStable => GetBytes(ReadProtocol.FakeStable),
                Response.MoveStable => GetBytes(ReadProtocol.MoveStable),
                Response.TiltStable => GetBytes(ReadProtocol.TiltStable),
                Response.Tap => GetBytes(ReadProtocol.Tap),
                Response.DoubleTap => GetBytes(ReadProtocol.DoubleTap),
                Response.ChargingStarted => GetBytes(ReadProtocol.Charging).Append((byte) 1).ToArray(),
                Response.ChargingStopped => GetBytes(ReadProtocol.Charging).Append((byte) 0).ToArray(),
                Response.Color => WithData(ReadProtocol.Color, data),
                _ => throw new ArgumentOutOfRangeException(nameof(response), response, null)
            };
        }

        private static byte[] WithData(string key, IEnumerable<byte> data)
        {
            var result = GetBytes(key).ToList();
            result.AddRange(data);
            return result.ToArray();
        }

        private static byte[] GetBytes(string str) => UTF8.GetBytes(str);
    }
}