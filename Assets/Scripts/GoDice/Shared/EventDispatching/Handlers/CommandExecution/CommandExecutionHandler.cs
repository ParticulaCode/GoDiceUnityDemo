using System;
using FrostLib.Commands;
using GoDice.Shared.EventDispatching.Events;
using JetBrains.Annotations;

namespace GoDice.Shared.EventDispatching.Handlers.CommandExecution
{
    [UsedImplicitly]
    public class CommandExecutionHandler<T> : EventHandler where T : ICommand
    {
        public CommandExecutionHandler(IEvent ev) : base(ev)
        {
        }

        public override void Handle() => Activator.CreateInstance<T>().Execute();
    }
}