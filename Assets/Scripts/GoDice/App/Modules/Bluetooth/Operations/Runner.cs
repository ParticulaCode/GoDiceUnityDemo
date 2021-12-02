using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Containers;
using FrostLib.Coroutines;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.Utils;

namespace GoDice.App.Modules.Bluetooth.Operations
{
    ///     I know that using Stack looks weird. But.
    ///     When we send a bulk of the same commands to all dice (like battery request),
    /// it doesn't matter in what oreder we schedule them, since they all scheduled almost
    /// the same frame.
    ///     Yet when we establish connection, it's better to go one by one. Eg. connect to the die,
    /// then execute all inialization for that die, then proceed to connection to the next die.
    /// When we scan, we get all connected dice at wance, so we schedule all connections at once.
    /// Stack allows us to avoid initialization delayes and make it seem smooth.
    ///     Also when disconnection happens unintentionally, we want to execute reconnection operation
    /// to the die immediately. Stack allows to it without reorganizing the collection.
    internal partial class Runner : IRunner
    {
        private static IRoutineRunner RoutineRunner => ServiceLocator.Instance.Get<IRoutineRunner>();

        private readonly IBluetoothBridge _bridge;
        private readonly ExpireContainer _timeoutAwaiter;
        private readonly StopwatchAdapter _stopwatch = new StopwatchAdapter();

        private Stack<IOperation> _collection = new Stack<IOperation>();
        private bool _rountineRunning;
        private IOperation _currentOperation;
        private bool _pruneCurrentOperation;

        public Runner(IBluetoothBridge bridge, int timeout)     
        {
            _bridge = bridge;
            _timeoutAwaiter = new ExpireContainer(timeout);
        }

        public void Schedule(IOperation op)
        {
            if (_collection.Any(o => o.Equals(op)))
            {
                LogOperationDiscarded(op);
                return;
            }

            _collection.Push(op);
            LogOperationScheduled(op);

            if (!_rountineRunning)
                RoutineRunner.StartRoutine(Run());
        }

        private IEnumerator Run()
        {
            _rountineRunning = true;

            while (_collection.Count > 0)
            {
                _currentOperation = _collection.Pop();
                _timeoutAwaiter.Refresh();
                _stopwatch.Start();

                LogPerformStart();
                SafePerform(_currentOperation);

                while (!_currentOperation.IsDone)
                {
                    if (_timeoutAwaiter.IsExpired)
                    {
                        LogOperationTimeouted();
                    }
                    else if (_pruneCurrentOperation)
                    {
                        LogCurrentOperationPruned();
                    }
                    else
                    {
                        yield return null;

                        continue;
                    }

                    _currentOperation.Abort();
                    break;
                }

                var endTime = _stopwatch.End();
                LogIfOperationIsDone(_currentOperation, endTime);

                _currentOperation = null;
                _pruneCurrentOperation = false;
            }

            _rountineRunning = false;
        }

        private void SafePerform(IOperation op)
        {
            try
            {
                op.Perform(_bridge);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public void PruneOperaionsByAddress(string address)
        {
            if (_currentOperation != null && _currentOperation.CanBePruned(address))
                _pruneCurrentOperation = true;

            if(_collection.Count == 0)
                return;
            
            var toPrune = _collection.Where(o => !o.Equals(_currentOperation) && o.CanBePruned(address))
                .ToArray();

            if (toPrune.Length == 0)
                return;

            var list = _collection.ToList();
            list.RemoveAll(o => toPrune.Contains(o));
            list.Reverse();
            _collection = new Stack<IOperation>(list);

            LogPrunedOperations(toPrune);
        }
    }
}