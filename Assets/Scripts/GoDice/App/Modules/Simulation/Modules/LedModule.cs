using System;
using System.Collections;
using System.Collections.Generic;
using FrostLib.Containers;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class LedModule : Module
    {
        [ReadOnly] [SerializeField] private Color[] _leds = new Color[2];

        private enum Mode
        {
            Toggle,
            Constant,
            Off
        }

        private Coroutine _toggleRoutine;

        public void Parse(IReadOnlyList<sbyte> data)
        {
            var mode = ParseMode(data[0]);
            switch (mode)
            {
                case Mode.Toggle:
                    DeactivateLeds();
                    StartToggle(data);
                    break;
                case Mode.Constant:
                    SetColor(0, ToColor(data, 1));
                    SetColor(1, ToColor(data, 4));
                    break;
                case Mode.Off:
                    DeactivateLeds();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Mode ParseMode(sbyte data) =>
            data switch
            {
                WriteProtocol.Led.Toggle => Mode.Toggle,
                WriteProtocol.Led.Constant => Mode.Constant,
                WriteProtocol.Led.Off => Mode.Off,
                _ => throw new NotImplementedException($"Can't detect mode {data}")
            };

        private void DeactivateLeds()
        {
            SetAllLeds(Color.black);

            if (_toggleRoutine != null)
                StopCoroutine(_toggleRoutine);

            _toggleRoutine = null;
        }

        private void SetAllLeds(Color color)
        {
            for (var i = 0; i < _leds.Length; i++)
                SetColor(i, color);
        }

        private void SetColor(int idx, Color color) => _leds[idx] = color;

        private void StartToggle(IReadOnlyList<sbyte> data) =>
            _toggleRoutine = StartCoroutine(ToggleRoutined(
                (byte) data[1],
                data[2] / 100f,
                data[3] / 100f,
                ToColor(data, 4),
                Convert.ToBoolean(data[7]),
                data[8]));

        private Color ToColor(IReadOnlyList<sbyte> data, int startIdx)
        {
            var r = Normalize(data[startIdx]);
            var g = Normalize(data[startIdx + 1]);
            var b = Normalize(data[startIdx + 2]);
            var color = new Color(r, g, b);
            return color;
        }

        //We write byte into sbyte and back again because BT plugin work only with sbytes
        private float Normalize(sbyte data) => (float) (byte) data / byte.MaxValue;

        private IEnumerator ToggleRoutined(byte rounds, float onDuration, float offDuration,
            Color color, bool mixed, sbyte ledsToActivate)
        {
            var componentIdx = new CircularIndex(0, 0, 2);
            
            var ledIdx = new CircularIndex(0, 0, 1);
            if (ledsToActivate != WriteProtocol.Led.BothLeds)
            {
                var idx = ledsToActivate - 1;
                ledIdx = new CircularIndex(idx, idx, idx);
            }

            var isInfinite = rounds == 255;
            while (rounds > 0 || isInfinite)
            {
                if (mixed)
                {
                    SetAllLeds(color);
                    yield return new WaitForSeconds(onDuration);
                }
                else
                {
                    SetColor(ledIdx, new Color { [componentIdx] = color[componentIdx], a = 1 });
                    yield return new WaitForSeconds(onDuration);

                    ledIdx.MoveNext();
                    componentIdx.MoveNext();
                }

                SetAllLeds(Color.black);
                yield return new WaitForSeconds(offDuration);

                rounds--;
            }
        }
    }
}