using UnityEngine;

namespace FrostLib.Containers.Limiters
{
    public class NonNegativeValuesOnlyLimiter : ILimiter
    {
        public int Limit(int value) => Mathf.Max(value, 0);
    }
}