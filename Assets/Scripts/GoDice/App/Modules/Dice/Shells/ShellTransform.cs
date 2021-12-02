using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public class ShellTransform : IShellTransformer
    {
        private readonly Shell _shell;
        private readonly Dictionary<int, int> _transformations;
        private readonly int[] _possibleValues;

        public ShellTransform(Shell shell, Dictionary<int, int> transformations)
        {
            _shell = shell;
            _transformations = transformations;
            var pv = new List<int>();
            foreach(var pair in _transformations)
            {
                if(!pv.Contains(pair.Value))
                    pv.Add(pair.Value);
            }
            _possibleValues = pv.ToArray();
        }

        public int AxisToValue(Vector3 axis) => _transformations[_shell.AxisToValue(axis)];

        public Vector3 ValueToAxis(int value)
        {
            var proxyValue = _transformations.FirstOrDefault(pair => pair.Value == value).Key;
            return _shell.ValueToAxis(proxyValue);
        }

        public int[] PossibleValues() => (int[])_possibleValues.Clone();

        public int GetSensitivity() => _shell.GetSensitivity();
    }
}