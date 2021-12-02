using System;

namespace FrostLib.Commands
{
    public class ExecuteOnceCommand : ICommand
    {
        private readonly Action _action;
        private bool _isExecuted;

        public ExecuteOnceCommand(Action action) => _action = action;

        public void Execute()
        {
            if (_isExecuted)
                return;

            _isExecuted = true;
            _action?.Invoke();
        }

        public void Reset() => _isExecuted = false;
    }
}