using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoDice.App.Modules.Dice.Messaging;
using GoDice.App.Modules.Dice.Shells;
using GoDice.Shared.Data;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class RollModule : Module
    {
        [ReadOnly] [SerializeField] private int _value;

        [EnumToggleButtons]
        [HideLabel]
        [SerializeField]
        private ShellType _shell = ShellType.D6;

        [PropertyRange(nameof(MinValue), nameof(MaxValue))]
        [SerializeField]
        private int _predefinedValue = 1;

        private int MinValue => _shell == ShellType.D10X ? 0 : 1;
        private int MaxValue => (int) _shell;

        [Range(0f, 3f)] [SerializeField] private float _rollDuration = 1f;

        private static class Buttons
        {
            public const string Title = "Actions";
            public const string Vertical = Title + "/Buttons";
            public const string Row1 = Vertical + "/Row1";
            public const string Row2 = Vertical + "/Row2";
        }

        [GUIColor(0, 1f, 0)]
        [TitleGroup(Buttons.Title, "or you can use hotkeys...")]
        [VerticalGroup(Buttons.Vertical)]
        [HorizontalGroup(Buttons.Row1)]
        [Button("Roll", ButtonSizes.Large)]
        public void Roll() => Roll(GetRandomValue());

        [GUIColor(0.8f, 0.4f, 0.2f)]
        [HorizontalGroup(Buttons.Row1)]
        [Button("Predefined", ButtonSizes.Large)]
        private void RollPredefined() => Roll(_predefinedValue);

        public void Roll(int value)
        {
            SendRollStart();
            SendRollEndRoutined(value);
        }

        private void SendRollStart() => SendData(Reader.BuildResponse(Response.Roll));

        private void SendRollEndRoutined(int value, Response response = Response.RollEnd) =>
            WaitAndExecute(_rollDuration, () =>
            {
                _value = value;
                SendStable(response);
            });


        private void WaitAndExecute(float time, Action cb) =>
            StartCoroutine(WaitAndExecuteRoutined(time, cb));

        private static IEnumerator WaitAndExecuteRoutined(float time, Action cb)
        {
            yield return new WaitForSeconds(time);

            cb();
        }

        private void SendStable(Response response)
        {
            var data = Reader.BuildResponse(response);
            var shell = ShellHolder.GetShell(_shell);
            var vector = shell.ValueToAxis(_value);
            var bytes = VectorToBytes(vector);
            data = data.Concat(bytes).ToArray();
            SendData(data);
        }

        private static IEnumerable<byte> VectorToBytes(Vector3 vector) =>
            new[]
            {
                (byte) Mathf.RoundToInt(vector.x),
                (byte) Mathf.RoundToInt(vector.y),
                (byte) Mathf.RoundToInt(vector.z)
            };

        private int GetRandomValue(int[] except = null)
        {
            var shell = ShellHolder.GetShell(_shell);
            var values = shell.PossibleValues();
            if (except != null)
                values = values.Except(except).ToArray();

            var rand = UnityEngine.Random.Range(0, values.Length);
            return values[rand];
        }

        [GUIColor(0.9f, 0.52f, 0.9f)]
        [HorizontalGroup(Buttons.Row1)]
        [Button("Rotate", ButtonSizes.Large)]
        public void Rotate() => SendRollEndRoutined(GetRandomValue(new[] { _value }));

        [GUIColor(0, .5f, 1f)]
        [HorizontalGroup(Buttons.Row2)]
        [Button("Move", ButtonSizes.Large)]
        public void Move()
        {
            SendRollStart();
            SendRollEndRoutined(GetRandomValue(), Response.MoveStable);
        }

        [GUIColor(0, .5f, 1f)]
        [HorizontalGroup(Buttons.Row2)]
        [Button("Tilt", ButtonSizes.Large)]
        public void Tilt() => SendRollEndRoutined(GetRandomValue(), Response.TiltStable);

        [GUIColor(0, .5f, 1f)]
        [HorizontalGroup(Buttons.Row2)]
        [Button("Cheat", ButtonSizes.Large)]
        public void Cheat() => SendRollEndRoutined(GetRandomValue(), Response.FakeStable);
    }
}