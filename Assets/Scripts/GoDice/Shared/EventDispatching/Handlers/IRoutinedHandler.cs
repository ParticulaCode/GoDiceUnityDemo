using System.Collections;

namespace GoDice.Shared.EventDispatching.Handlers
{
    public interface IRoutinedHandler
    {
        IEnumerator Handle();
    }
}