using Cysharp.Threading.Tasks;
using FrostLib.Coroutines;
using FrostLib.Services;
using FrostLib.Tasks;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;

namespace Demo
{
    internal class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            var servicer = ServiceLocator.Instance;
            var routineRunner = RoutineRunner.Create();
            servicer.Provide((IRoutineRunner) routineRunner);

            var cancellationTokenFactory =
                new CancellationTokenFactory(routineRunner.GetCancellationTokenOnDestroy());
            var dispatcher =
                (IEventDispatcher) new EventDispatcher(routineRunner, cancellationTokenFactory);
            servicer.Provide(dispatcher);

            new GoDice.App.Modules.Platforms.Bootstrapper().Load(dispatcher);
            GetComponent<GoDice.App.Modules.Simulation.Bootstrapper>().Load(servicer);
            GetComponent<GoDice.App.Modules.Bluetooth.Bootstrapper>().Load(servicer, dispatcher);
            GetComponent<GoDice.App.Modules.Dice.Bootstrapper>().Load(servicer, dispatcher);

            servicer.Provide(new GoDice.Shared.EventDispatching.Dispatching.Logger(dispatcher));
        }
    }
}