using System.Collections.Generic;
using System.Linq;
using GoDice.Utils;

namespace GoDice.App.Modules.Bluetooth.Operations
{
    internal partial class Runner
    {
        private void LogOperationScheduled(IOperation op) =>
            Log.Message($"{op} scheduled to perform. Operations in queue: {_collection.Count}");

        private static void LogOperationDiscarded(IOperation op) =>
            Log.Message($"{op} discarded. Same operation is already scheduled.");

        private void LogIfOperationIsDone(IOperation op, float endTime)
        {
            if (!op.IsDone)
                return;

            Log.Message($"{op} performed. Duration: {endTime} sec. Operations in queue: {_collection.Count}");
        }

        private void LogPerformStart() =>
            Log.Message($"Performing {_currentOperation}. Operations in queue: {_collection.Count}");

        private void LogCurrentOperationPruned() =>
            Log.Message(Colorizer.AsError($"{_currentOperation} aborted by prune request."));

        private void LogOperationTimeouted() =>
            Log.Message(Colorizer.AsError(
                $"{_currentOperation} aborted by timeout [{_timeoutAwaiter.TTL} sec]."));

        private static void LogPrunedOperations(IReadOnlyCollection<IOperation> toPrune)
        {
            var stringedOperations = string.Join(",", toPrune.Select(o => o.ToString()));
            Log.Message($"Operations pruned: {toPrune.Count}. {stringedOperations}");
        }
    }
}