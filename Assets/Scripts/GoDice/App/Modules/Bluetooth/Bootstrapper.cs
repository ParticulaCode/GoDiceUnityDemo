using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Bridge;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Bluetooth.Operations;
using GoDice.App.Modules.Bluetooth.Scan;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;
using static GoDice.Shared.EventDispatching.Events.EventType;

namespace GoDice.App.Modules.Bluetooth
{
    [AddComponentMenu("GoDice/App/[Bluetooth] Module Bootstrapper")]
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Settings _settings;

        public void Load(IProvider provider, IEventDispatcher dispatcher)
        {
            var bridge = new SetupBridgeCommand().Execute();
            provider.Provide(bridge);

            var deviceHolder = (IDeviceHolder) new Holder();
            provider.Provide(deviceHolder);

            provider.Provide((IScanner) new ScanOperator(bridge, dispatcher, _settings.DeviceFilter));

            var operationsRunner = new Runner(bridge, _settings.OperationTimeout);
            var reconnector = new ReconnectProcessor(operationsRunner);
            var connestionEstablisher = new ConnestionEstablisher(operationsRunner, reconnector);
            provider.Provide((IDeviceConnector) connestionEstablisher);

            dispatcher.Bind<ConnectToDeviceRequestHandler>().To(ConnectToDeviceRequest);
            dispatcher.Bind<ScanStartHandler>().To(NewDevicesScanStart);
        }
    }
}