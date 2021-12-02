using GoDice.App.Modules.Bluetooth.Bridge;

namespace GoDice.App.Modules.Bluetooth.Operations
{
    internal interface IOperation
    {
        bool IsDone { get; }

        void Perform(IBluetoothBridge bridge);
        void Abort();
        
        bool CanBePruned(string address);
    }
}