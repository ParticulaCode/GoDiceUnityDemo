using GoDice.App.Modules.Simulation;
using MoreLinq;
using UnityEngine;

namespace Demo
{
    internal class ControlPanel : MonoBehaviour
    {
        [SerializeField] private AddDieWindow _addDieWindow;

        public void StartScan() => _addDieWindow.Show();

        public void RollSimulatedDice() =>
            FindObjectOfType<BluetoothBridge>().ConnectedDevices.ForEach(d => d.Roll());
    }
}