using System.Linq;
using GoDice.App.Modules.Simulation;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoDice.Games.Editor.Tools
{
    [CreateAssetMenu(fileName = "Rotate Tools", menuName = "GoDice/Editor/Dice/Rotate Tools",
        order = 1)]
    public class RotateTools : ScriptableObject
    {
        private const string Hotkeys = "GoDice/Hotkeys/Rotate/";
        
        private const string Title = "Click on die to rotate it to a random value";
        private const string VerticalGroup = Title + "/Split";
        private const string Row1Title = VerticalGroup + "/Row1";
        private const string Row2Title = VerticalGroup + "/Row2";

        [TitleGroup(Title, "Or use hotkeys Ctrl+F1..F6")] 
        [VerticalGroup(VerticalGroup)]
        [HorizontalGroup(Row1Title)]
        [MenuItem(Hotkeys + "Die 1 %F1")]
        [Button(ButtonSizes.Large, Name = "Die 1")]
        public static void RotateDie1() => Rotate(0);

        [HorizontalGroup(Row1Title)]
        [MenuItem(Hotkeys + "Die 2 %F2")]
        [Button(ButtonSizes.Large, Name = "Die 2")]
        public static void RotateDie2() => Rotate(1);

        [HorizontalGroup(Row1Title)]
        [MenuItem(Hotkeys + "Die 3 %F3")]
        [Button(ButtonSizes.Large, Name = "Die 3")]
        public static void RotateDie3() => Rotate(2);
        
        [HorizontalGroup(Row2Title)]
        [MenuItem(Hotkeys + "Die 4 %F4")]
        [Button(ButtonSizes.Large, Name = "Die 4")]
        public static void RotateDie4() => Rotate(3);

        [HorizontalGroup(Row2Title)]
        [MenuItem(Hotkeys + "Die 5 %F5")]
        [Button(ButtonSizes.Large, Name = "Die 5")]
        public static void RotateDie5() => Rotate(4);

        [HorizontalGroup(Row2Title)]
        [MenuItem(Hotkeys + "Die 6 %F6")]
        [Button(ButtonSizes.Large, Name = "Die 6")]
        public static void RotateDie6() => Rotate(5);

        private static void Rotate(int dieIdx) => GetDie(dieIdx).Rotate();

        private static IDie GetDie(int dieIdx) => FindObjectOfType<BluetoothBridge>()
            .ConnectedDevices.Skip(dieIdx).First();
    }
}