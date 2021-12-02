using GoDice.Shared.Data;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public class ShellController : IShellController
    {
        private IShellTransformer _transformer;

        public int GetValue(Vector3 axis) => _transformer.AxisToValue(axis);

        public void SetShell(ShellType type) => _transformer = ShellHolder.GetShell(type);

        public sbyte GetSensitivity() => (sbyte) _transformer.GetSensitivity();
    }
}