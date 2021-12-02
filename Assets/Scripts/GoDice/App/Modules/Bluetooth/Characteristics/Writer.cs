using GoDice.App.Modules.Bluetooth.Operations;

namespace GoDice.App.Modules.Bluetooth.Characteristics
{
    internal class Writer
    {
        private readonly IRunner _runner;
        private readonly Characteristic _characteristic;

        public Writer(Characteristic characteristic, IRunner runner)
        {
            _characteristic = characteristic;
            _runner = runner;
        }

        public void Send(sbyte[] data, OperationType type) =>
            _runner.Schedule(new WriteOperation(_characteristic, data, type));
    }
}