using System;
using System.Collections;
using FrostLib.Commands.Routined;
using GoDice.Shared.EventDispatching.Events;
using JetBrains.Annotations;

namespace GoDice.Shared.EventDispatching.Handlers.CommandExecution
{
    [UsedImplicitly]
    public class RoutinedCommandExecutionHandler<T> : RoutinedEventHandler where T : IRoutinedCommand
    {
        public RoutinedCommandExecutionHandler(IEvent ev) : base(ev)
        {
        }

        public override IEnumerator Handle()
        {
            yield return Activator.CreateInstance<T>().Execute();
        }
    }
}