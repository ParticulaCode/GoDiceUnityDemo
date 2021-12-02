using System.Collections;

namespace GoDice.Shared.EventDispatching
{
    public interface IRoutinedHandler
    {
        IEnumerator Handle();
    }
}