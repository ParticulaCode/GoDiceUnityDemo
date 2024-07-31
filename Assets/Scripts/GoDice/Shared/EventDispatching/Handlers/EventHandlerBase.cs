using GoDice.Shared.EventDispatching.Events;
using MoreLinq;

namespace GoDice.Shared.EventDispatching.Handlers
{
    public abstract class EventHandlerBase : IDebugInfoProvider
    {
        string IDebugInfoProvider.DebugInfo
        {
            get
            {
                var type = GetType();
                return type.IsGenericType
                    ? $"{type.Namespace}{type.Name}<{type.GenericTypeArguments.ToDelimitedString(",")}>"
                    : type.UnderlyingSystemType.FullName;
            }
        }

        private readonly IEvent _event;

        protected EventHandlerBase(IEvent ev) => _event = ev;

        protected T EventAs<T>() where T : class => _event as T;

        protected bool EventIs<T>() => _event is T;
    }
}