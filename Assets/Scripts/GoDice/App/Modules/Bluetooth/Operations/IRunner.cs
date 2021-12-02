namespace GoDice.App.Modules.Bluetooth.Operations
{
    internal interface IRunner
    {
        void Schedule(IOperation op);
        void PruneOperaionsByAddress(string address);
    }
}