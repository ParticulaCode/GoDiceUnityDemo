using UnityEngine;

namespace GoDice.App.Modules.Dice.Led
{
    public interface ILedController
    {
        void Blink(ToggleMode mode);

        void Blink(int blinksAmount, Color color, float onDuration, float offDuration,
            bool isMixed);

        void OpenLed(OpenMode mode);

        void CloseAllLeds();

        sbyte[] GetMessage(ToggleMode mode);
    }
}