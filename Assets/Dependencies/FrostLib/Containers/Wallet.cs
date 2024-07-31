using System;
using FrostLib.Signals.impl;
using UnityEngine;

namespace FrostLib.Containers
{
    [Serializable]
    public class Wallet : IWallet
    {
        public int Value { get; private set; }

        public Signal<int> OnValueChangedSignal { get; } = new Signal<int>();

        public Wallet(int startingValue) => Value = startingValue;

        public virtual void Add(int value) => Set(Value + value);

        protected virtual void Set(int newValue)
        {
            Value = Mathf.Max(newValue, 0);
            OnValueChangedSignal.Dispatch(Value);
        }

        public bool Has(int value) => value <= Value;

        public bool TrySpend(int value)
        {
            if (!Has(value))
                return false;

            Spend(value);
            return true;
        }

        private void Spend(int value) => Set(Value - value);
    }
}