using System;
using System.Collections;
using FrostLib.Scenes;

namespace FrostLib.Coroutines
{
    internal class StopOnSceneSwitched
    {
        private readonly InterruptWrapper _wrapper;
        private readonly ExecuteOnSceneSwitchedOnce _executor;

        public StopOnSceneSwitched(IEnumerator enumerator, Action stopCallback)
        {
            _executor = new ExecuteOnSceneSwitchedOnce(OnSceneChanged);
            _wrapper = new InterruptWrapper(enumerator, stopCallback, _executor.Dispose);
        }

        public IEnumerator Start() => _wrapper.Start();

        private void OnSceneChanged() => _wrapper.Stop();
    }
}