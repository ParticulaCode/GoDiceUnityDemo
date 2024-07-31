using System.Threading;
using Cysharp.Threading.Tasks;

namespace GoDice.Shared.EventDispatching.Handlers
{
    public interface ITaskHandler
    {
        UniTask Handle(CancellationToken cancellationToken = default);
    }
}