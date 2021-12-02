using System.Linq;
using GoDice.Shared.Data;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Data
{
    [CreateAssetMenu(fileName = "ColorMap", menuName = "GoDice/Dice/Color Map", order = 0)]
    internal class ColorMap : ScriptableObject, IDieColorMapper
    {
        [SerializeField] private Record[] _map;

        [System.Serializable]
        private struct Record
        {
            public ColorType Type;
            public Color Color;
            public string LocaliztionKey;
        }

        public Color GetColor(ColorType type) => GetRecord(type).Color;

        private Record GetRecord(ColorType type) => _map.First(m => m.Type == type);

        public string GetColorLocalizaionKey(ColorType type) => GetRecord(type).LocaliztionKey;
    }
}