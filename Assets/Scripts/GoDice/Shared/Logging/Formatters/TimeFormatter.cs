using UnityEngine;

namespace GoDice.Shared.Logging.Formatters
{
    public class TimeFormatter : ILogFormatter
    {
        /// <summary>
        /// We can use alternative DateTime.Now:hh:mm:ss, but this version has more precision
        /// and I expect it to create less memory allocations
        /// </summary>
        public string Format(string msg) => $"[T: {Time.realtimeSinceStartup}] {msg}";
    }
}