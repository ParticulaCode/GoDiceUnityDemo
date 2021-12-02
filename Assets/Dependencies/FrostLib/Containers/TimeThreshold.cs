using System;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public abstract class TimeThreshold
    {
        [SerializeField]
        private float _threshold = 0.1f;

        private float LastActivationTime { get; set; } = -100f;

        protected TimeThreshold() { }

        protected TimeThreshold(float threshold) => _threshold = threshold;

        public bool Activate()
        {
            var time = GetTime();
            if (time - LastActivationTime < _threshold)
                return false;

            Reset();
            return true;
        }

        public void Reset() => LastActivationTime = GetTime();

        protected abstract float GetTime();
    }
}