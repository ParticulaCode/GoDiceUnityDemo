//http://wiki.unity3d.com/index.php/CoroutineHelper

using System.Collections;
using FrostLib.Services;
using UnityEngine;

namespace FrostLib.Coroutines
{
    public class Run
    {
        public bool IsDone { get; private set; }
        public bool IsAborted { get; private set; }

        private IEnumerator _action;

        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IRoutineRunner Routiner => Servicer.Get<IRoutineRunner>();

        #region Run.EachFrame

        public static Run EachFrame(System.Action action)
        {
            var tmp = new Run();
            tmp._action = RunEachFrame(tmp, action);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator RunEachFrame(Run run, System.Action action)
        {
            run.IsDone = false;
            while (true)
            {
                if (!run.IsAborted && action != null)
                    action();
                else
                    break;

                yield return null;
            }
            run.IsDone = true;
        }

        #endregion Run.EachFrame

        #region Run.Every

        public static Run Every(float initialDelay, float delay, System.Action action)
        {
            var tmp = new Run();
            tmp._action = RunEvery(tmp, initialDelay, delay, action);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator RunEvery(Run aRun, float aInitialDelay, float aSeconds, System.Action aAction)
        {
            aRun.IsDone = false;
            if (aInitialDelay > 0f)
            {
                yield return new WaitForSeconds(aInitialDelay);
            }
            else
            {
                var frameCount = Mathf.RoundToInt(-aInitialDelay);
                for (var i = 0; i < frameCount; i++)
                    yield return null;
            }

            while (true)
            {
                if (!aRun.IsAborted && aAction != null)
                    aAction();
                else
                    break;

                if (aSeconds > 0)
                    yield return new WaitForSeconds(aSeconds);
                else
                {
                    var frameCount = Mathf.Max(1, Mathf.RoundToInt(-aSeconds));
                    for (var i = 0; i < frameCount; i++)
                        yield return null;
                }
            }

            aRun.IsDone = true;
        }

        #endregion Run.Every

        #region Run.After

        public static Run After(float delay, System.Action action)
        {
            var tmp = new Run();
            tmp._action = RunAfter(tmp, delay, action);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator RunAfter(Run aRun, float aDelay, System.Action aAction)
        {
            aRun.IsDone = false;

            yield return new WaitForSeconds(aDelay);

            if (!aRun.IsAborted)
                aAction?.Invoke();

            aRun.IsDone = true;
        }

        #endregion Run.After

        #region Run.Lerp

        public static Run Lerp(float duration, System.Action<float> action)
        {
            var tmp = new Run();
            tmp._action = RunLerp(tmp, duration, action);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator RunLerp(Run aRun, float aDuration, System.Action<float> aAction)
        {
            aRun.IsDone = false;

            var t = 0f;
            while (t < 1.0f)
            {
                t = Mathf.Clamp01(t + Time.deltaTime / aDuration);
                if (!aRun.IsAborted)
                    aAction?.Invoke(t);

                yield return null;
            }

            aRun.IsDone = true;
        }

        #endregion Run.Lerp

        #region Run.OnDelegate

        public static Run OnDelegate(SimpleEvent @delegate, System.Action action)
        {
            var tmp = new Run();
            tmp._action = RunOnDelegate(tmp, @delegate, action);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator RunOnDelegate(Run run, SimpleEvent callback, System.Action action)
        {
            run.IsDone = false;
            callback.Add(action);

            while (!run.IsAborted && action != null)
                yield return null;

            callback.Remove(action);
            run.IsDone = true;
        }

        #endregion Run.OnDelegate

        #region Run.Coroutine

        public static Run Coroutine(IEnumerator coroutine)
        {
            var tmp = new Run();
            tmp._action = Coroutine(tmp, coroutine);
            tmp.Start();
            return tmp;
        }

        private static IEnumerator Coroutine(Run run, IEnumerator coroutine)
        {
            yield return Routiner.StartRoutine(coroutine);
            run.IsDone = true;
        }

        #endregion Run.Coroutine

        private void Start()
        {
            if (_action != null)
                Routiner.StartRoutine(_action);
        }

        public Coroutine Wait => Routiner.StartRoutine(WaitFor(null));

        public IEnumerator WaitFor(System.Action onDone)
        {
            while (!IsDone && !IsAborted)
                yield return null;

            onDone?.Invoke();
        }

        public void Abort() => IsAborted = true;

        public Run ExecuteWhenDone(System.Action action)
        {
            var tmp = new Run { _action = WaitFor(action) };
            tmp.Start();
            return tmp;
        }
    }
}