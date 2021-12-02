using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Bridge;
using UnityEngine;

namespace GoDice.App.Modules.Simulation
{
    [AddComponentMenu("GoDice/App/Simulation Module Bootstrapper")]
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private BluetoothBridge _simulatedBluetooth;

        public void Load(IProvider servicer)
        {
#if UNITY_EDITOR
            ProvideBridge(servicer);
#endif
        }

        private void ProvideBridge(IProvider servicer)
        {
            var bridge = FindObjectOfType<BluetoothBridge>();
            if (bridge == null)
                bridge = Instantiate(_simulatedBluetooth);

            servicer.Provide((IBluetoothBridge) bridge);
        }
    }
}