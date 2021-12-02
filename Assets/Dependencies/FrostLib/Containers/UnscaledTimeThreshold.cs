using System;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public class UnscaledTimeThreshold : TimeThreshold
    {
        public UnscaledTimeThreshold() { }

        public UnscaledTimeThreshold(float threshold) : base(threshold) { }

        public static UnscaledTimeThreshold CreateAndActivate(float threshold)
        {
            var t = new UnscaledTimeThreshold(threshold);
            t.Activate();
            return t;
        }

        protected override float GetTime() => Time.unscaledTime;
    }
}
