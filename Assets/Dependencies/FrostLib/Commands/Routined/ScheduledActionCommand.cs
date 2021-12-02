using System;
using System.Collections;

namespace FrostLib.Commands.Routined
{
    public class ScheduledActionCommand : ICommand
    {
        private readonly Action _action;

        public ScheduledActionCommand(Action action) => _action = action;

        public IEnumerator Execute()
        {
            _action?.Invoke();
            yield return null;
        }
    }
}