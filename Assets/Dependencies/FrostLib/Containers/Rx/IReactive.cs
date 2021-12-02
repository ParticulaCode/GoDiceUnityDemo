using System;
using FrostLib.Signals.impl;

namespace FrostLib.Containers.Rx
{
    //Renamed due to annoying conflict with System.IObservable<out T>
    public interface IReactive<T>: IDisposable
    {
        T Value { get; }
        Signal<T> OnChange { get; }
    }
}