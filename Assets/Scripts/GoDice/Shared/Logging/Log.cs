using System.Collections.Generic;
using System.Linq;
using FrostLib.Extensions;
using GoDice.Shared.Logging.Formatters;

namespace GoDice.Shared.Logging
{
    public enum Mode
    {
        Bluetooth,
        Operations,
        Dice,
    }

    public static class Log
    {
        private static readonly Mode[] ModesToUse =
        {
#if BLUETOOTH_DEBUG
            Mode.Bluetooth,
#endif
#if BLUETOOTH_OPERATIONS_DEBUG
            Mode.Operations,
#endif
#if DICE_DEBUG
            Mode.Dice,
#endif
        };

        private static readonly Dictionary<Mode, ILogger> Loggers =
            new Dictionary<Mode, ILogger>();

        static Log()
        {
            var mockup = (ILogger) new Mockup();
            var frameFormatter = new IfNotEditorFormatter(new FrameFormatter());
            var timeFormatter = new IfNotEditorFormatter(new TimeFormatter());

            foreach (var mode in Enum<Mode>.GetEnumValues())
            {
                var logger = ModesToUse.Contains(mode)
                    ? new FormattedLogger(new ILogFormatter[]
                    {
                        frameFormatter,
                        timeFormatter,
                        new IfDebugBuildFormatter(new TagFormatter($"#{mode}#"))
                    })
                    : mockup;

                Loggers.Add(mode, logger);
            }
        }

        public static void Message(string msg, Mode mode) => Loggers[mode].Log(msg);

        public static void Error(string msg, Mode mode) => Loggers[mode].LogError(msg);
    }

    internal class Mockup : ILogger
    {
        public void Log(string msg)
        {
        }

        public void LogError(string msg)
        {
        }
    }
}