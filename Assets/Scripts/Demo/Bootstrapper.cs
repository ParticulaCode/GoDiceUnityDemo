using FrostLib.Coroutines;
using FrostLib.Services;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;

namespace Demo
{
    internal class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            var servicer = ServiceLocator.Instance;
            var routineRunner = (IRoutineRunner) RoutineRunner.Create();
            servicer.Provide(routineRunner);

            var dispatcher = (IEventDispatcher) new EventDispatcher(routineRunner);
            servicer.Provide(dispatcher);

            GetComponent<GoDice.App.Modules.Simulation.Bootstrapper>().Load(servicer);
            GetComponent<GoDice.App.Modules.Bluetooth.Bootstrapper>().Load(servicer, dispatcher);
            GetComponent<GoDice.App.Modules.Dice.Bootstrapper>().Load(servicer, dispatcher);
        }
    }
}