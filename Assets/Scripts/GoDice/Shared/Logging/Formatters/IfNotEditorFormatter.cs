namespace GoDice.Shared.Logging.Formatters
{
    public class IfNotEditorFormatter : ILogFormatter
    {
        private readonly ILogFormatter _slave;

        public IfNotEditorFormatter(ILogFormatter slave) => _slave = slave;

        public string Format(string msg)
        {
#if !UNITY_EDITOR
            return _slave.Format(msg);
#endif
            return msg;
        }
    }
}