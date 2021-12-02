using GoDice.Shared.Data;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public interface IShellController
    {
        int GetValue(Vector3 axis);
        void SetShell(ShellType type);
        sbyte GetSensitivity();
    }
}