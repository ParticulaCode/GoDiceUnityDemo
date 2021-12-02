namespace GoDice.App.Modules.Bluetooth.Operations
{
    public enum OperationType
    {
        Connection,
        LedToggle,
        LedConstant,
        LedOff,
        BatteryRequest,
        SensitivitySet,
        TapInterruptOpen,
        TapInterruptClose,
        DoubleTapInterruptOpen,
        DoubleTapInterruptClose,
        ColorRequest,
        Initialization,
        RollDetectionSettings
    }
}