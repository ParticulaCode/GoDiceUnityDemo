using System;
using System.Collections;

namespace FrostLib.Coroutines
{
    public class InterruptWrapper
    {
        private readonly Action _onFinish;
        private readonly IEnumerator _enumerator;
        private readonly Action _stopCallback;

        private bool _isActive;

        public InterruptWrapper(IEnumerator enumerator, Action stopCallback,
            Action onFinish = null)
        {
            _enumerator = enumerator;
            _stopCallback = stopCallback;
            _onFinish = onFinish;
        }

        public IEnumerator Start()
        {
            _isActive = true;
            
            while (_isActive && _enumerator.MoveNext())
                yield return _enumerator.Current;
            
            _onFinish?.Invoke();
        }

        public void Stop()
        {
            _isActive = false;
            _stopCallback?.Invoke();
        }
    }
}