namespace GoDice.Shared.EventDispatching.Events
{
    public enum EventType
    {
        None = 0,

        /// Android
        RequestAndroidPermission = 150,

        /// Bluetooth
        ConnectToDeviceRequest = 200,
        BluetoothDeviceConnected = 201,
        NewDevicesScanStart = 205,

        /// Dice
        DieChargingStateChanged = 1000,
        DieClicked = 1003,
        DieConnected = 1006,
        DieDisconnected = 1007,
        DieShellChanged = 1009,

        DieStartedRoll = 2000,
        DieEndedRoll = 2010,
        DieStable = 2011,
        DieRotated = 2020,
        DieTap = 2025,
        DieDoubleTap = 2026,
        BlinkDie = 2027,

        DieExpectTap = 2040,
        DieIgnoreTap = 2041,
        DieExpectDoubleTap = 2042,
        DieIgnoreDoubleTap = 2043
    }
}