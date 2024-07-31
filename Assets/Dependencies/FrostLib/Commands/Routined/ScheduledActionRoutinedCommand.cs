using System;
using System.Collections;

namespace FrostLib.Commands.Routined
{
    public class ScheduledActionRoutinedCommand : IRoutinedCommand
    {
        private readonly Action _action;

        public ScheduledActionRoutinedCommand(Action action) => _action = action;

        public IEnumerator Execute()
        {
            _action?.Invoke();
            yield return null;
        }
    }
}