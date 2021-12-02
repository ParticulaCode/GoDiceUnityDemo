using System;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Dice.Core;

namespace GoDice.App.Modules.Dice.Connectors
{
    internal abstract class OnChangeDieListener<T> : IDisposable
    {
        protected readonly Die Die;

        private readonly Signal<T> _onChangeSignal;

        protected OnChangeDieListener(Die die, Signal<T> onChangeSignal)
        {
            Die = die;
            _onChangeSignal = onChangeSignal;
            _onChangeSignal.AddListener(OnChange);
        }

        protected abstract void OnChange(T isOn);

        public void Dispose() => _onChangeSignal.RemoveListener(OnChange);
    }
}