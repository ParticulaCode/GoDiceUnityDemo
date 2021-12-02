namespace GoDice.Shared.Logging
{
    public interface ILogger
    {
        void Log(string msg);
        void LogError(string msg);
    }
}