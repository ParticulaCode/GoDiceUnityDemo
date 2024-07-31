using System.Collections;
using GoDice.Shared.EventDispatching.Events;
using UnityEngine;

namespace GoDice.Shared.EventDispatching.Handlers
{
    internal class WaitForEndOfFrameEventHandler : EventHandlerBase, IRoutinedHandler
    {
        public WaitForEndOfFrameEventHandler(IEvent ev) : base(ev)
        {
        }

        public IEnumerator Handle()
        {
            yield return new WaitForEndOfFrame();
        }
    }
}