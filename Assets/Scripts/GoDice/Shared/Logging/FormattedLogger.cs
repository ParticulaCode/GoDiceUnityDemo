using System.Collections.Generic;
using System.Linq;
using GoDice.Shared.Logging.Formatters;
using JetBrains.Annotations;
using UnityEngine;

namespace GoDice.Shared.Logging
{
    [UsedImplicitly]
    public class FormattedLogger : ILogger
    {
        private readonly ILogFormatter[] _formatters;

        public FormattedLogger(IEnumerable<ILogFormatter> formatters) =>
            _formatters = formatters.Reverse().ToArray();

        public void Log(string msg) => Debug.Log(Format(msg));

        public void LogError(string msg) => Debug.LogError(Format(msg));

        private string Format(string msg) =>
            _formatters.Aggregate(msg, (current, formatter) => formatter.Format(current));
    }
}