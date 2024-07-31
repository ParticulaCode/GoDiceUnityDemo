using UnityEngine;

namespace FrostLib.Containers.Limiters
{
    public class FromZeroToMaxLimiter : ILimiter
    {
        private readonly int _maxValue;

        public FromZeroToMaxLimiter(int maxValue) => _maxValue = maxValue;

        public int Limit(int value) => Mathf.Clamp(value, 0, _maxValue);
    }
}