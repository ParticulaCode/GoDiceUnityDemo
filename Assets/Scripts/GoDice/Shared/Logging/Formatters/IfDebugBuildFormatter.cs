namespace GoDice.Shared.Logging.Formatters
{
    public class IfDebugBuildFormatter : ILogFormatter
    {
        private readonly ILogFormatter _slave;

        public IfDebugBuildFormatter(ILogFormatter slave) => _slave = slave;

        public string Format(string msg) => UnityEngine.Debug.isDebugBuild ? _slave.Format(msg) : msg;
    }
}