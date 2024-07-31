using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FrostLib.Commands.Async;
using GoDice.Shared.EventDispatching.Events;
using JetBrains.Annotations;

namespace GoDice.Shared.EventDispatching.Handlers.CommandExecution
{
    [UsedImplicitly]
    public class TaskCommandExecutionHandler<T> : TaskEventHandler where T : ITaskCommand
    {
        public TaskCommandExecutionHandler(IEvent ev) : base(ev)
        {
        }

        public override UniTask Handle(CancellationToken cancellationToken = default) =>
            Activator.CreateInstance<T>().Execute(cancellationToken);
    }
}