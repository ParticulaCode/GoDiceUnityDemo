using System;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public class ScaledTimeThreshold : TimeThreshold
    {
        public ScaledTimeThreshold() { }

        public ScaledTimeThreshold(float threshold) : base(threshold) { }

        public static ScaledTimeThreshold CreateAndActivate(float threshold)
        {
            var t = new ScaledTimeThreshold(threshold);
            t.Activate();
            return t;
        }

        protected override float GetTime() => Time.time;
    }
}