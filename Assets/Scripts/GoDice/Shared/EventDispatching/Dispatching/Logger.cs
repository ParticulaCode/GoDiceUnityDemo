using System;
using GoDice.Shared.EventDispatching.Handlers;
using GoDice.Shared.Logging;
using UnityEngine;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    //HANDLERS_DEBUG
    public class Logger : IDisposable
    {
        private readonly IEventDispatcher _dispatcher;

        public Logger(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _dispatcher.OnRaisingEventSignal.AddListener(LogRaising);

            if (!Debug.isDebugBuild)
                return;

            _dispatcher.OnHandlerCreatedSignal.AddListener(LogCreation);
        }

        private static void LogRaising(Events.EventType evType) =>
            Log.Message($"Raising: {PrintEvent(evType)}", Mode.Handlers);

        private static void LogCreation(EventHandlerBase handler, Events.EventType evType)
        {
            var message = "\t=> Handling, but handler doesn't implement IDebugInfoProvider";
            if (handler is IDebugInfoProvider debugable)
                message = $"\t=> Handling {PrintEvent(evType)}: {debugable.DebugInfo}";

            Log.Message(message, Mode.Handlers);
        }

        private static string PrintEvent(Events.EventType evType) => $"[{evType}]";

        public void Dispose()
        {
            _dispatcher.OnRaisingEventSignal.RemoveListener(LogRaising);

            if (!Debug.isDebugBuild)
                return;

            _dispatcher.OnHandlerCreatedSignal.RemoveListener(LogCreation);
        }
    }
}