using FrostLib.Services;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Debugging;
using GoDice.App.Modules.Dice.Presentation;
using GoDice.Shared.EventDispatching.Binding;
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
            dispatcher.Bind()
                .Handler<DieConnectionHandler>()
                .Handler<UpdateConnectDicePresentationHandler>()
                .To(DieConnected);

            dispatcher.Bind()
                .Handler<DieConnectionHandler>()
                .Handler<UpdateConnectDicePresentationHandler>()
                .To(DieDisconnected);

            dispatcher.Bind().Handler<DieRollHandler>().To(DieStartedRoll);
            dispatcher.Bind().Handler<DieRollHandler>().To(DieEndedRoll);
            dispatcher.Bind().Handler<DieRollHandler>().To(DieStable);

            dispatcher.Bind().Handler<BlinkDieHandler>().To(BlinkDie);
            dispatcher.Bind().Handler<BlinkDieOnClickHandler>().To(DieClicked);

            dispatcher.Bind().Handler<SwitchDieTapExpectationHandler>().To(DieExpectTap);
            dispatcher.Bind().Handler<SwitchDieTapExpectationHandler>().To(DieIgnoreTap);
            dispatcher.Bind().Handler<SwitchDieTapExpectationHandler>().To(DieExpectDoubleTap);
            dispatcher.Bind().Handler<SwitchDieTapExpectationHandler>().To(DieIgnoreDoubleTap);

            dispatcher.Bind().Handler<CreateDieForNewDeviceHandler>().To(BluetoothDeviceConnected);

            dispatcher.Bind().Handler<LogDieEventEventHandler>().To(DieStartedRoll);
            dispatcher.Bind().Handler<LogDieEventEventHandler>().To(DieEndedRoll);
            dispatcher.Bind().Handler<LogDieEventEventHandler>().To(DieStable);
            dispatcher.Bind().Handler<LogDieEventEventHandler>().To(DieRotated);
        }
    }
}