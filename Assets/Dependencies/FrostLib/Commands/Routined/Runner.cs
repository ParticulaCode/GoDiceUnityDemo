using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Coroutines;
using FrostLib.Signals.impl;
using UnityEngine;

namespace FrostLib.Commands.Routined
{
    public class Runner : IDisposable
    {
        public readonly Signal OnAllExecutionsFinished = new Signal();

        public bool IsActive = true;
        public bool IsEmpty => _scheduled.Count == 0 && _activeRoutine == null;

        private readonly Queue<ICommand> _scheduled = new Queue<ICommand>();

        private readonly IRoutineRunner _routiner;

        private Coroutine _activeRoutine;
        private bool _executionFinished;

        public Runner(IRoutineRunner routiner)
        {
            _routiner = routiner;
            _routiner.StartRoutine(ProcessRoutined());
        }

        public void Schedule(ICommand cmd)
        {
            Log($"\tSchedule: {cmd}");
            _executionFinished = false;
            _scheduled.Enqueue(cmd);
        }

        private IEnumerator ProcessRoutined()
        {
            var wait = new WaitForEndOfFrame();
            while (true)
            {
                if (IsEmpty || !IsActive)
                {
                    HandleExecutionFinished();

                    yield return wait;

                    continue;
                }

                var cmd = _scheduled.Dequeue();
                Log($"\tExecuting: {cmd}");

                _activeRoutine = _routiner.StartRoutine(cmd.Execute());
                yield return _activeRoutine;

                _activeRoutine = null;
                Log($"\tExecution finished: {cmd}");
            }
        }

        private void HandleExecutionFinished()
        {
            if (_executionFinished)
                return;

            _executionFinished = true;
            OnAllExecutionsFinished.Dispatch();
        }

        private void ClearScheduled()
        {
            Log(
                $"\tClearScheduled. Commands in queue {string.Join(", ", _scheduled.Select(cmd => cmd))}");
            _scheduled.Clear();
        }

        public void Dispose()
        {
            IsActive = false;
            OnAllExecutionsFinished.ClearListeners();

            var routiner = _routiner;
            if (_activeRoutine != null && routiner != null && !routiner.Equals(null))
                routiner.StopRoutine(_activeRoutine);

            ClearScheduled();
        }

        private static void Log(string text)
        {
#if COMMANDS_RUNNER_DEBUG
            Debug.Log(text);
#endif
        }
    }
}