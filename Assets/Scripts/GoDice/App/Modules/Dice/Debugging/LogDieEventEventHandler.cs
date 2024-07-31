using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Handlers;
using GoDice.Shared.EventDispatching.Injections;
using GoDice.Shared.Events.Dice;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice.Debugging
{
    [UsedImplicitly]
    internal class LogDieEventEventHandler : EventHandler
    {
        [Inject] private ILoggersHolder Loggers { get; set; }

        private readonly EventType _evType;

        public LogDieEventEventHandler(DieEvent ev) : base(ev) => _evType = ev.Type;

        public override void Handle()
        {
            var dieId = EventAs<DieEvent>().DieId;
            var logger = Loggers.Get(dieId);
            logger?.LogEvent(_evType);
        }
    }
}