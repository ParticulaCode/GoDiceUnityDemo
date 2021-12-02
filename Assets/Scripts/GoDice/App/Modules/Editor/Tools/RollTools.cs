using System.Collections.Generic;
using System.Linq;
using GoDice.App.Modules.Simulation;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoDice.Games.Editor.Tools
{
    [CreateAssetMenu(fileName = "Roll Tools", menuName = "GoDice/Editor/Dice/Roll Tools",
        order = 0)]
    public class RollTools : ScriptableObject
    {
        private const string RollHotkeys = "GoDice/Hotkeys/Roll/";

        private static readonly List<(int DieIdx, int? Value)> BatchRollDice =
            new List<(int dieIdx, int? Value)>();

        #region Roll all dice

        private static class RollAllDice
        {
            public const string Hotkeys = RollHotkeys + "All/";
            public const string Title = "Roll all dice:";
        }

        [TitleGroup(RollAllDice.Title)]
        [MenuItem(RollAllDice.Hotkeys + "Random &r")]
        [Button(ButtonSizes.Large, Name = "Random (ALT+R)")]
        public static void RollDice()
        {
            foreach (var die in GetDice())
                die.Roll();
        }

        private static IEnumerable<IDie> GetDice() =>
            FindObjectOfType<BluetoothBridge>().ConnectedDevices;

        [TitleGroup(RollAllDice.Title)]
        [PropertySpace(SpaceAfter = 30)]
        [Button("To target number (ALT+F1..F6)", ButtonSizes.Large, Style = ButtonStyle.Box)]
        private void RollToTarget(int number) => RollAllTo(number);

        [MenuItem(RollAllDice.Hotkeys + "To 1 &F1")]
        private static void RollAllTo1() => RollAllTo(1);

        [MenuItem(RollAllDice.Hotkeys + "To 2 &F2")]
        private static void RollAllTo2() => RollAllTo(2);

        [MenuItem(RollAllDice.Hotkeys + "To 3 &F3")]
        private static void RollAllTo3() => RollAllTo(3);

        [MenuItem(RollAllDice.Hotkeys + "To 4 &F4")]
        private static void RollAllTo4() => RollAllTo(4);

        [MenuItem(RollAllDice.Hotkeys + "To 5 &F5")]
        private static void RollAllTo5() => RollAllTo(5);

        [MenuItem(RollAllDice.Hotkeys + "To 6 &F6")]
        private static void RollAllTo6() => RollAllTo(6);

        private static void RollAllTo(int value)
        {
            foreach (var die in GetDice())
                die.Roll(value);
        }

        #endregion

        #region Add to the batch roll

        private static class AddToBatch
        {
            public const string Title = "Click to add a die (by index) to the batch roll:";
            public const string Vertical = Title + "/Split";
            public const string Row1 = Vertical + "/Row1";
            public const string Row2 = Vertical + "/Row2";
            public const string Hotkeys = RollHotkeys + "Add to the batch/";
        }

        [VerticalGroup(AddToBatch.Vertical)]
        [HorizontalGroup(AddToBatch.Row1)]
        [MenuItem(AddToBatch.Hotkeys + "Die 1 _F1")]
        [Button("Die 1", ButtonSizes.Large)]
        [TitleGroup(AddToBatch.Title,
            "In a batch roll only selected dice will roll. Use F1..F6 hotkeys to select the die.")]
        private static void AddToBatchRoll_1() => AddToBatchRoll(0);

        [HorizontalGroup(AddToBatch.Row1)]
        [MenuItem(AddToBatch.Hotkeys + "Die 2 _F2")]
        [Button("Die 2", ButtonSizes.Large)]
        private static void AddToBatchRoll_2() => AddToBatchRoll(1);

        [HorizontalGroup(AddToBatch.Row1)]
        [MenuItem(AddToBatch.Hotkeys + "Die 3 _F3")]
        [Button("Die 3", ButtonSizes.Large)]
        private static void AddToBatchRoll_3() => AddToBatchRoll(2);

        [HorizontalGroup(AddToBatch.Row2)]
        [MenuItem(AddToBatch.Hotkeys + "Die 4 _F4")]
        [Button("Die 4", ButtonSizes.Large)]
        private static void AddToBatchRoll_4() => AddToBatchRoll(3);

        [HorizontalGroup(AddToBatch.Row2)]
        [MenuItem(AddToBatch.Hotkeys + "Die 5 _F5")]
        [Button("Die 5", ButtonSizes.Large)]
        private static void AddToBatchRoll_5() => AddToBatchRoll(4);

        [HorizontalGroup(AddToBatch.Row2)]
        [MenuItem(AddToBatch.Hotkeys + "Die 6 _F6")]
        [Button("Die 6", ButtonSizes.Large)]
        private static void AddToBatchRoll_6() => AddToBatchRoll(5);

        public static void AddToBatchRoll(int dieIdx) => BatchRollDice.Add((dieIdx, null));

        #endregion

        #region Select predefined value

        private static class SelectNumber
        {
            public const string Title =
                "Click to assign a roll result for the last die added to the batch:";
            public const string Vertical = Title + "/Split";
            public const string Row1 = Vertical + "/Row1";
            public const string Row2 = Vertical + "/Row2";
            public const string Hotkeys = RollHotkeys + "Select predefined value/";
        }

        [HorizontalGroup(SelectNumber.Row1)]
        [VerticalGroup(SelectNumber.Vertical)]
        [Button("1", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "1 #1")]
        [TitleGroup(SelectNumber.Title, "Or you can press Shift+Number.")]
        private static void SelectPredefinedValue_1() => SelectPredefinedValue(1);

        [HorizontalGroup(SelectNumber.Row1)]
        [Button("2", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "2 #2")]
        private static void SelectPredefinedValue_2() => SelectPredefinedValue(2);

        [HorizontalGroup(SelectNumber.Row1)]
        [Button("3", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "3 #3")]
        private static void SelectPredefinedValue_3() => SelectPredefinedValue(3);

        [HorizontalGroup(SelectNumber.Row2)]
        [Button("4", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "4 #4")]
        private static void SelectPredefinedValue_4() => SelectPredefinedValue(4);

        [HorizontalGroup(SelectNumber.Row2)]
        [Button("5", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "5 #5")]
        private static void SelectPredefinedValue_5() => SelectPredefinedValue(5);

        [HorizontalGroup(SelectNumber.Row2)]
        [Button("6", ButtonSizes.Large)]
        [MenuItem(SelectNumber.Hotkeys + "6 #6")]
        public static void SelectPredefinedValue_6() => SelectPredefinedValue(6);

        private static void SelectPredefinedValue(int value)
        {
            var diceInBatchAmount = BatchRollDice.Count;
            if (diceInBatchAmount == 0)
                return;

            var lastElementIdx = diceInBatchAmount - 1;
            var (idx, _) = BatchRollDice[lastElementIdx];
            BatchRollDice[lastElementIdx] = (idx, value);
        }

        #endregion

        [MenuItem(RollHotkeys + "Batch roll _`")]
        [Button("Batch roll.", ButtonSizes.Large)]
        public static void BatchRoll()
        {
            var simulatedDice = GetDice().ToArray();
            for (var i = 0; i < simulatedDice.Length; i++)
            {
                var hasBind = BatchRollDice.Any(t => t.DieIdx == i);
                if (!hasBind)
                    continue;

                var simDie = simulatedDice[i];
                var (_, predefinedValue) = BatchRollDice.FirstOrDefault(t => t.DieIdx == i);

                if (predefinedValue.HasValue)
                    simDie.Roll(predefinedValue.Value);
                else
                    simDie.Roll();
            }

            BatchRollDice.Clear();
        }

        #region Preset

        private static class Preset
        {
            public const string Title = "Roll to preset result:";
            public const string Vertical = Title + "/Split";
        }

        [TitleGroup(Preset.Title)]
        [VerticalGroup(Preset.Vertical)]
        public int[] PresetResult;

        [VerticalGroup(Preset.Vertical)]
        [Button("Roll dice to preset result.", ButtonSizes.Large)]
        private void RollPreset() => Roll(PresetResult);

        #endregion

        public static void Roll(int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                AddToBatchRoll(i);
                SelectPredefinedValue(values[i]);
            }

            BatchRoll();
        }
    }
}