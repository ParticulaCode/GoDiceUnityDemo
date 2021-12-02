using System;
using FrostLib.Signals.impl;

namespace FrostLib.Containers.Rx
{
    /// Interface can't have impicit cast operator. Having type Property.Value everywhere is annoying
    /// And it hurts readability. More code for no reason. And "if(IsOn.Value)" is terrible
    public class ReadonlyReactive<T> : IReactive<T>
    {
        public static implicit operator T(ReadonlyReactive<T> self) => self.Value;

        public T Value => _reactive.Value;
        public Signal<T> OnChange => _reactive.OnChange;

        private readonly IReactive<T> _reactive;

        public ReadonlyReactive(IReactive<T> reactive) => _reactive = reactive;

        public override string ToString() => Value.ToString();
        
        //This class doesn't own the signal to dispose to.
        void IDisposable.Dispose() { }
    }
}