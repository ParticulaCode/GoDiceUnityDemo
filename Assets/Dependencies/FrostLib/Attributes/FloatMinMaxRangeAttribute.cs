using System;

namespace FrostLib.Attributes
{
    public class FloatMinMaxRangeAttribute : Attribute
    {
        public FloatMinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; private set; }
        public float Max { get; private set; }
    }
}