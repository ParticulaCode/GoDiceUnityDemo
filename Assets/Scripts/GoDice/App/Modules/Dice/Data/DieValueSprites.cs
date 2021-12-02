using System.Linq;
using FrostLib.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoDice.App.Modules.Dice.Data
{
    [CreateAssetMenu(fileName = "DieValues", menuName = "GoDice/Dice/Die Values",
        order = 0)]
    public class DieValueSprites : ScriptableObject
    {
        [FormerlySerializedAs("_map")]
        [SerializeField]
        private Sprite[] _values;
        [SerializeField] private Sprite _unknownValue;
        [SerializeField] private Sprite _shellSprite;

        public Sprite ShellSprite => _shellSprite;

        public Sprite GetValueSprite(int value)
        {
            var index = value - 1;
            if (index > -1 && index < _values.Length)
                return _values[index];

            return _unknownValue;
        }

        public Sprite GetRandomValueSpriteExcept(Sprite spr) =>
            _values.Where(s => s != spr).ToArray().PickRandom();
    }
}