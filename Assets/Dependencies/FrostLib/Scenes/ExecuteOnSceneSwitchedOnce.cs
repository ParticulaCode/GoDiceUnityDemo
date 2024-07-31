using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine.SceneManagement;

namespace FrostLib.Scenes
{
    //Unsubscribing a lot of callbacks one by one from SceneManager.activeSceneChanged is expensive
    //Essentially it's find and remove from Array. Profiler prooved.
    //That is why only one callback and local hashed collection is used
    public class ExecuteOnSceneSwitchedOnce : IDisposable
    {
        private static readonly Dictionary<Guid, Action> Subs = new();
        private Guid _guid;

        static ExecuteOnSceneSwitchedOnce() => SceneManager.activeSceneChanged += OnSceneChanged;

        public ExecuteOnSceneSwitchedOnce()
        {
        }

        public ExecuteOnSceneSwitchedOnce(Action action) => SetAction(action);

        public void SetAction(Action action)
        {
            _guid = Guid.NewGuid();
            Subs.Add(_guid, action);
        }

        private static void OnSceneChanged(Scene _, Scene __)
        {
            if (!Subs.Any())
                return;

            var tempSubs = Subs.Values.ToArray();
            Subs.Clear();
            tempSubs.ForEach(action => action.Invoke());
        }

        public void Dispose()
        {
            if (!Subs.ContainsKey(_guid))
                return;

            Subs.Remove(_guid);
        }
    }
}