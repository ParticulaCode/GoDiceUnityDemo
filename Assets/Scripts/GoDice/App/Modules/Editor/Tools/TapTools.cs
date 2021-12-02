using System.Linq;
using GoDice.App.Modules.Simulation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoDice.Games.Editor.Tools
{
    [CreateAssetMenu(fileName = "Tap Tools", menuName = "GoDice/Editor/Dice/Tap Tools",
        order = 1)]
    public class TapTools : ScriptableObject
    {
        private const string Title = "Click on die to tap it";
        private const string VerticalGroup = Title + "/Split";
        private const string Row1Title = VerticalGroup + "/Row1";
        private const string Row2Title = VerticalGroup + "/Row2";

        [TitleGroup(Title)]
        [VerticalGroup(VerticalGroup)]
        [HorizontalGroup(Row1Title)]
        [Button(ButtonSizes.Large, Name = "Die 1")]
        public static void TapDie1() => Tap(0);

        [HorizontalGroup(Row1Title)]
        [Button(ButtonSizes.Large, Name = "Die 2")]
        public static void TapDie2() => Tap(1);

        [HorizontalGroup(Row1Title)]
        [Button(ButtonSizes.Large, Name = "Die 3")]
        public static void TapDie3() => Tap(2);

        [HorizontalGroup(Row2Title)]
        [Button(ButtonSizes.Large, Name = "Die 4")]
        public static void TapDie4() => Tap(3);

        [HorizontalGroup(Row2Title)]
        [Button(ButtonSizes.Large, Name = "Die 5")]
        public static void TapDie5() => Tap(4);

        [HorizontalGroup(Row2Title)]
        [Button(ButtonSizes.Large, Name = "Die 6")]
        public static void TapDie6() => Tap(5);

        private static void Tap(int dieIdx) => GetDie(dieIdx).Tap();

        private static IDie GetDie(int dieIdx) => FindObjectOfType<BluetoothBridge>()
            .ConnectedDevices.Skip(dieIdx).First();
    }
}