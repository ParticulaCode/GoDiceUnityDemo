using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrostLib.Coroutines
{
    //Create, then start
    public class NestedCoroutine
    {
        private bool _terminated;

        private IEnumerator _routine;
        private readonly List<NestedCoroutine> _nested = new List<NestedCoroutine>();
        private MonoBehaviour _mb;
        private Func<NestedCoroutine, IEnumerator> _func;

        public NestedCoroutine(MonoBehaviour mb, Func<NestedCoroutine, IEnumerator> func)
        {
            _mb = mb;
            _func = func;
        }

        public void Start()
        {
            _routine = _func(this);
            StartCoroutine(Wrapper());
        }

        private Coroutine StartCoroutine(IEnumerator routine) => _mb.StartCoroutine(routine);

        private IEnumerator Wrapper()
        {
            while (!_terminated && _routine.MoveNext())
                yield return _routine.Current;

            _terminated = true;
            Dispose();
        }

        private void Dispose()
        {
            _routine = null;
            _nested.Clear();
            _mb = null;
            _func = null;
        }

        public Coroutine WaitFor() => StartCoroutine(Wait());

        private IEnumerator Wait()
        {
            while (!_terminated)
                yield return null;
        }

        public IEnumerator Nest(Func<NestedCoroutine, IEnumerator> func)
        {
            var child = new NestedCoroutine(_mb, func);
            _nested.Add(child);
            child.Start();
            return child.Wait();
        }

        public void Stop()
        {
            _terminated = true;

            foreach (var child in _nested)
                child.Stop();
        }
    }

    public static class MonoBehaviourExtension
    {
        public static NestedCoroutine CreatNestedCoroutine(this MonoBehaviour mb, 
            Func<NestedCoroutine, IEnumerator> func)
        {
            return new NestedCoroutine(mb, func);
        }
    }
}