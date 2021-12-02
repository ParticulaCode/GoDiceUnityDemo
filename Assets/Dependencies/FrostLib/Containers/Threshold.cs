using System;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public class Threshold
    {
        [SerializeField]
        private float _limit;

        public Threshold(float limit) => _limit = limit;

        public bool IsLowerAbs(float value) => _limit < Mathf.Abs(value);

        public bool IsHigherAbs(float value) => _limit > Mathf.Abs(value);
    }
}