namespace GoDice.Shared.EventDispatching.Events
{
    public interface IEvent
    {
        EventType Type { get; }
    }
}