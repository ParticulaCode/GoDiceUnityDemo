using System;
using System.Collections.Generic;

namespace GoDice.Shared.EventDispatching.Binding
{
    public class Group
    {
        public bool IsSequential;
        public bool UnbindAfterFirstExecution;
        public bool CancelOnSceneSwitch = true;
        public bool DisposeSceneCancellationHookAfterHandling;

        public IEnumerable<Type> Handlers => _handlers;

        private readonly List<Type> _handlers = new();

        public void Add(Type type) => _handlers.Add(type);

        internal Group Clone()
        {
            var clone = new Group
            {
                IsSequential = IsSequential,
                CancelOnSceneSwitch = CancelOnSceneSwitch,
                DisposeSceneCancellationHookAfterHandling = DisposeSceneCancellationHookAfterHandling
            };

            foreach (var handler in Handlers)
                clone.Add(handler);

            return clone;
        }
    }
}