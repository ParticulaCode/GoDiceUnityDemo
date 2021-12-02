using FrostLib.Collections;
using FrostLib.Signals.impl;
using GoDice.App.Modules.Bluetooth.Devices;

namespace GoDice.App.Modules.Bluetooth.Scan
{
    public class Sync<T>
    {
        public readonly SignaledList<T> Collection;
        public readonly Signal OnScanFinished;

        public Sync(SignaledList<T> collection, Signal onScanFinished)
        {
            Collection = collection;
            OnScanFinished = onScanFinished;
        }
    }
}