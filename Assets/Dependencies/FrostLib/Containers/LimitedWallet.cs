using System;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public class LimitedWallet : Wallet
    {
        private readonly int _maxValue;

        public LimitedWallet(int startingValue, int maxValue) : base(startingValue) => _maxValue = maxValue;

        protected override void Set(int newValue) => base.Set(Mathf.Min(newValue, _maxValue));
    }
}