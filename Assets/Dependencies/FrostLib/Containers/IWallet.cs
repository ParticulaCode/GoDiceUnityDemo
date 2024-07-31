using FrostLib.Signals.impl;

namespace FrostLib.Containers
{
    public interface IWallet
    {
        int Value { get; }
        Signal<int> OnValueChangedSignal { get; }

        void Add(int value);
        bool Has(int value);
        bool TrySpend(int value);
    }
}