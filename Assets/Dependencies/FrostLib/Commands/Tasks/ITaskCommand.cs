using System.Threading;
using Cysharp.Threading.Tasks;

namespace FrostLib.Commands.Async
{
    public interface ITaskCommand
    {
        UniTask Execute(CancellationToken cancellationToken = default);
    }

    public interface ITaskCommand<T>
    {
        UniTask<T> Execute(CancellationToken cancellationToken = default);
    }
}