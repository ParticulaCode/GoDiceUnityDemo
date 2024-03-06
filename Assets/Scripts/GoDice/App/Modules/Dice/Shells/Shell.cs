using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public class Shell : IShellTransformer
    {
        private readonly Dictionary<int, Vector3> _values;
        private readonly int _sensitivity;

        public Shell(Dictionary<int, Vector3> values, int sensitivity)
        {
            _values = values;
            _sensitivity = sensitivity;
        }

        public int AxisToValue(Vector3 axis)
        {
            var value = 0;
            var distance = float.MaxValue;
            foreach (var pair in _values)
            {   
                Vector3 offset = axis - pair.Value;
                var lDist = offset.sqrMagnitude;
                if (lDist >= distance)
                    continue;

                value = pair.Key;
                distance = lDist;
            }

            return value;
        }

        public Vector3 ValueToAxis(int value) => _values[value];

        public int[] PossibleValues() => _values.Keys.ToArray();

        public int GetSensitivity() => _sensitivity;
    }
}
