using UnityEngine;

namespace GoDice.App.Modules.Bluetooth
{
    [CreateAssetMenu(fileName = "BluetoothSettings",
        menuName = "GoDice/App/Bluetooth/Settings", 
        order = 0)]
    internal class Settings : ScriptableObject
    {
        [Tooltip("Seconds")]
        [SerializeField] public int OperationTimeout = 10;
        [SerializeField] public string DeviceFilter = "GoDice.*";
    }
}