using System;
using FrostLib.Signals.impl;

namespace FrostLib.Containers
{
    public class DisposableGroup : IDisposable
    {
        private readonly Signal _disposeSignal = new();

        public virtual void Dispose() => _disposeSignal.Dispatch();

        public void Add(Action action) => _disposeSignal.AddOnce(action);
    }
}