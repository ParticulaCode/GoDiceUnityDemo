using System.Collections;
using FrostLib.Scenes;
using UnityEngine;

namespace FrostLib.Coroutines
{
    public class RoutineRunner : MonoBehaviour, IRoutineRunner
    {
        private bool IsValid => !_isDestroyed && gameObject != null;

        /// Workaround for Unity bullshit.
        /// It's possible somehow to get NellRefException on gameObject != null check if it's destroyed
        /// Yet it should not be happening, GetComponent should return null if object is destroyed.
        /// This happens when Editor stops playing. Possible Editor only case
        private bool _isDestroyed;

        public static RoutineRunner Create()
        {
            var instance = new GameObject("RoutineRunner").AddComponent<RoutineRunner>();
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }

        public Coroutine StartRoutine(IEnumerator routine, bool stopOnSceneSwitch)
        {
            if (!stopOnSceneSwitch)
                return StartCoroutine(routine);

            var executeOnce = new ExecuteOnSceneSwitchedOnce();
            var targetRoutine = new StopOnSceneSwitched(routine, executeOnce.Dispose).Start();
            var coroutine = StartCoroutine(targetRoutine);
            executeOnce.SetAction(() => StopRoutine(coroutine));

            return coroutine;
        }

        public void StopRoutine(IEnumerator routine)
        {
            if (routine == null || !IsValid)
                return;

            StopCoroutine(routine);
        }

        public void StopRoutine(Coroutine routine)
        {
            if (routine == null || !IsValid)
                return;

            StopCoroutine(routine);
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
            StopAllCoroutines();
        }
    }
}