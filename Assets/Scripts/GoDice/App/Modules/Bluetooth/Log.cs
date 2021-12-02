using GoDice.Shared.Logging;

namespace GoDice.App.Modules.Bluetooth
{
    internal static class Log
    {
        public static void Message(string msg) => Shared.Logging.Log.Message(msg, Mode.Bluetooth);
        
        public static void Operation(string msg) => Shared.Logging.Log.Message(msg, Mode.Operations);
    }
}