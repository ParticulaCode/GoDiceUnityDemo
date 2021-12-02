using System.Diagnostics;

namespace GoDice.App.Modules.Bluetooth.Operations
{
    internal class StopwatchAdapter
    {
#if BLUETOOTH_DEBUG
        private readonly Stopwatch _watch = new Stopwatch();
#endif

        public void Start()
        {
#if BLUETOOTH_DEBUG
            _watch.Restart();
#endif
        }

        public float End()
        {
#if BLUETOOTH_DEBUG
            _watch.Stop();
            return (float) _watch.Elapsed.TotalSeconds;
#else
            return 0f;
#endif
        }
    }
}