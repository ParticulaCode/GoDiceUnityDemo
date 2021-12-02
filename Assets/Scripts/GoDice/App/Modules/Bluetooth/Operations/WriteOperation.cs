using System.Linq;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Characteristics;
using GoDice.Utils;
#if BLUETOOTH_OPERATIONS_DEBUG
using System.Collections.Generic;
using System.Text;

#endif

namespace GoDice.App.Modules.Bluetooth.Operations
{
    internal sealed class WriteOperation : IOperation
    {
        public bool IsDone { get; private set; }

        private readonly Characteristic _characteristic;
        private readonly OperationType _type;
        private readonly sbyte[] _data;

        private bool _isDone;

        public WriteOperation(Characteristic characteristic, sbyte[] data, OperationType type)
        {
            _characteristic = characteristic;
            _data = data;
            _type = type;
        }

        public void Perform(IBluetoothBridge bridge)
        {
#if BLUETOOTH_OPERATIONS_DEBUG
            Log.Operation(
                $"Writing: address {Colorizer.AsAddress(_characteristic.Address)} > {PrintBytes(_data)}");
#endif
            bridge.WriteCharacteristic(_characteristic, _data, true, OnSendSuccess);
        }

        public void Abort()
        {
        }

        public bool CanBePruned(string address) => _characteristic.Address == address;

        private void OnSendSuccess(string service) => IsDone = true;

        //TODO move it outside!
#if BLUETOOTH_OPERATIONS_DEBUG
        private static string PrintBytes(IReadOnlyList<sbyte> bytes)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Count; ++i)
            {
                if (i > 0)
                    builder.Append(" ");

                if (bytes[i] < 10)
                {
                    builder.Append("0");
                    builder.Append(bytes[i]);
                }
                else
                {
                    builder.Append(bytes[i]);
                }
            }

            return builder.ToString();
        }
#endif

        #region Object overrides

        public override bool Equals(object obj)
        {
            if (!(obj is WriteOperation other))
                return false;

            return Equals(other);
        }

        private bool Equals(WriteOperation other) =>
            _type == other._type
            && _characteristic.Equals(other._characteristic)
            && _data.SequenceEqual(other._data);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _characteristic.GetHashCode();
                hashCode = (hashCode * 397) ^ (_data != null ? _data.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) _type;
                return hashCode;
            }
        }

        public override string ToString() =>
            Colorizer.AsOperation(_characteristic.Address, _type.ToString());

        #endregion
    }
}