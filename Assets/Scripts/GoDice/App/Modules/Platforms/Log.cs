using GoDice.Shared.Logging;

namespace GoDice.App.Modules.Platforms
{
    internal static class Log
    {
        public static void Message(string msg) => Shared.Logging.Log.Message(msg, Mode.Platforms);
    }
}