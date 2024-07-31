using System.Threading;
using Cysharp.Threading.Tasks;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Exceptions;
using JetBrains.Annotations;

namespace GoDice.Shared.EventDispatching.Handlers
{
    [UsedImplicitly]
    public abstract class SequenceCancellationHandlerBase : TaskEventHandler
    {
        protected abstract bool IsCancellationRequired { get; }
        protected abstract string Message { get; }

        protected SequenceCancellationHandlerBase(IEvent ev) : base(ev)
        {
        }

        public override UniTask Handle(CancellationToken cancellationToken = default)
        {
            if (IsCancellationRequired)
                throw new SequenceCanceledException(Message);

            return UniTask.CompletedTask;
        }
    }
}