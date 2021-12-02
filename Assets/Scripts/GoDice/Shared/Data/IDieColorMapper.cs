using UnityEngine;

namespace GoDice.Shared.Data
{
    public interface IDieColorMapper
    {
        Color GetColor(ColorType type);
        string GetColorLocalizaionKey(ColorType type);
    }
}