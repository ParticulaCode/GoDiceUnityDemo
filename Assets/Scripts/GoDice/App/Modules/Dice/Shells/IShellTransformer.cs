using UnityEngine;

namespace GoDice.App.Modules.Dice.Shells
{
    public interface IShellTransformer
    {
        int AxisToValue(Vector3 axis);
        Vector3 ValueToAxis(int value);
        
        int[] PossibleValues();
        
        int GetSensitivity();
    }
}
