using FrostLib.Services;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Debugging;
using GoDice.App.Modules.Dice.Presentation;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;
using static GoDice.Shared.EventDispatching.Events.EventType;

namespace GoDice.App.Modules.Dice
{
    [AddComponentMenu("GoDice/App/[Dice] Module Bootstrapper")]
    public class Bootstrapper : MonoBehaviour
    {
        public void Load(ServiceLocator servicer, IEventDispatcher dispatcher)
        {
            ProvideServices(servicer);
            BindHandlers(dispatcher);
        }

        private void ProvideServices(IProvider servicer)
        {
            ProvideDiceService(servicer);

            servicer.Provide((IConnectedDicePresentersManager) new PresentersManager());
        }

        private static void ProvideDiceService(IProvider servicer)
        {
            var diceHolder = new Holder();
            servicer.Provide(diceHolder);
            servicer.Provide((IDiceHolder) diceHolder);
            servicer.Provide((ILoggersHolder) new LoggersHolder());
        }

        private static void BindHandlers(IEventDispatcher dispatcher)
        {
            dispatcher.Bind<DieConnectionHandler>().Then<UpdateConnectDicePresentationHandler>()
                .To(DieConnected);
            dispatcher.Bind<DieConnectionHandler>().Then<UpdateConnectDicePresentationHandler>()
                .To(DieDisconnected);

            dispatcher.Bind<DieRollHandler>().To(DieStartedRoll);
            dispatcher.Bind<DieRollHandler>().To(DieEndedRoll);
            dispatcher.Bind<DieRollHandler>().To(DieStable);

            dispatcher.Bind<BlinkDieHandler>().To(BlinkDie);
            dispatcher.Bind<BlinkDieOnClickHandler>().To(DieClicked);
            
            //dispatcher.Bind<DieChargingStateChangedHandler>().To(DieChargingStateChanged);
            dispatcher.Bind<SwitchDieTapExpectationHandler>().To(DieExpectTap);
            dispatcher.Bind<SwitchDieTapExpectationHandler>().To(DieIgnoreTap);
            dispatcher.Bind<SwitchDieTapExpectationHandler>().To(DieExpectDoubleTap);
            dispatcher.Bind<SwitchDieTapExpectationHandler>().To(DieIgnoreDoubleTap);

            dispatcher.Bind<CreateDieForNewDeviceHandler>().To(BluetoothDeviceConnected);

            dispatcher.Bind<LogDieEventEventHandler>().To(DieStartedRoll);
            dispatcher.Bind<LogDieEventEventHandler>().To(DieEndedRoll);
            dispatcher.Bind<LogDieEventEventHandler>().To(DieStable);
            dispatcher.Bind<LogDieEventEventHandler>().To(DieRotated);
        }
    }
}